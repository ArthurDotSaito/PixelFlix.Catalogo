﻿using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.Repository;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly PixelflixCatalogDbContext _context;
    private DbSet<Category> _categories => _context.Set<Category>();

    public CategoryRepository(PixelflixCatalogDbContext context)
    {
        _context = context;
    }

    public async Task Insert(Category anAggregate, CancellationToken aCancellationToken)
    {
        await _categories.AddAsync(anAggregate, aCancellationToken);
    }


    public Task Delete(Category anAggregate, CancellationToken aCancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Category> Get(Guid id, CancellationToken aCancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<SearchRepositoryResponse<Category>> Search(SearchRepositoryRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(Category anAggregate, CancellationToken aCancellationToken)
    {
        throw new NotImplementedException();
    }
}
