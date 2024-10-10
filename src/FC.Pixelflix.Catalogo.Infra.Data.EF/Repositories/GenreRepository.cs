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
    
    public Task Insert(Genre anAggregate, CancellationToken aCancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Genre> Get(Guid id, CancellationToken aCancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Genre anAggregate, CancellationToken aCancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(Genre anAggregate, CancellationToken aCancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<SearchRepositoryResponse<Genre>> Search(SearchRepositoryRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}