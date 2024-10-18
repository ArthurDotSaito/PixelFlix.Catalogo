using FC.Pixelflix.Catalogo.Application.Exceptions;
using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.Repository;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using FC.Pixelflix.Catalogo.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly PixelflixCatalogDbContext _context;
    private DbSet<Genre> _genres => _context.Set<Genre>();
    private DbSet<GenresCategories> _genresCategories => _context.Set<GenresCategories>();

    public GenreRepository(PixelflixCatalogDbContext context)
    {
        _context = context;
    }
    
    public async Task Insert(Genre anAggregate, CancellationToken aCancellationToken)
    {
        await _genres.AddAsync(anAggregate);
        if (anAggregate.Categories.Count > 0)
        {
                var relations = anAggregate.Categories.Select(categoryId => new GenresCategories(categoryId, anAggregate.Id));
                await _genresCategories.AddRangeAsync(relations);
        }
    }

    public async Task<Genre> Get(Guid id, CancellationToken aCancellationToken)
    {
        var aGenre = await _genres.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id, aCancellationToken);
        
        NotFoundException.ThrowIfNull(aGenre, $"Genre '{id}' was not found");
        
        var categoriesIds = await _genresCategories.Where(relation => relation.GenreId == id).Select(relation => relation.CategoryId).ToListAsync();
        
        categoriesIds.ForEach(aGenre.AddCategory);

        return aGenre;
    }

    public Task Delete(Genre anAggregate, CancellationToken aCancellationToken)
    {
        _genresCategories.RemoveRange(_genresCategories.Where(x=>x.GenreId == anAggregate.Id));
        _genres.Remove(anAggregate);
        
        return Task.CompletedTask;
    }

    public Task Update(Genre anAggregate, CancellationToken aCancellationToken)
    {
        _genres.Update(anAggregate);
        _genresCategories.RemoveRange(_genresCategories.Where(gc => gc.GenreId == anAggregate.Id));
        return Task.CompletedTask;
    }

    public Task<SearchRepositoryResponse<Genre>> Search(SearchRepositoryRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}