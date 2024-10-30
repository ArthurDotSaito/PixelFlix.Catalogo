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

    public async Task Update(Genre anAggregate, CancellationToken aCancellationToken)
    {
        _genres.Update(anAggregate);
        _genresCategories.RemoveRange(_genresCategories.Where(gc => gc.GenreId == anAggregate.Id));
        if (anAggregate.Categories.Count > 0)
        {
            var relations = anAggregate.Categories.Select(categoryId => new GenresCategories(categoryId, anAggregate.Id));
            await _genresCategories.AddRangeAsync(relations);
        }
    }

    public async Task<SearchRepositoryResponse<Genre>> Search(SearchRepositoryRequest request, CancellationToken cancellationToken)
    {
        var toSkip = (request.Page - 1) * request.PerPage;
        var genres = await _genres.Skip(toSkip).Take(request.PerPage).ToListAsync();
        var total = await _genres.CountAsync();
        var genresIds = genres.Select(genre => genre.Id).ToList();
        var relations = await _genresCategories.Where(relation => genresIds.Contains(relation.GenreId)).ToListAsync();
        var relationsByGenreId = relations.GroupBy(relation => relation.GenreId).ToList();
        
        relationsByGenreId.ForEach(relationGroup =>
        {
            var genreAggregate = genres.Find(genre =>genre.Id == relationGroup.Key);
            if (genreAggregate is null) return;
            relationGroup.ToList().ForEach(relation => genreAggregate.AddCategory(relation.CategoryId));
        });
        
        return new SearchRepositoryResponse<Genre>(request.Page, request.PerPage, total, genres);
    }
}