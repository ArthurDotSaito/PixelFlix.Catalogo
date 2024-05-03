﻿using FC.Pixelflix.Catalogo.Application.Exceptions;
using FC.Pixelflix.Catalogo.Domain.Entities;
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
    public async Task<Category> Get(Guid id, CancellationToken aCancellationToken)
    {
       var aCategory = await _categories.AsNoTracking().FirstOrDefaultAsync(category => category.Id == id, aCancellationToken);
       NotFoundException.ThrowIfNull(aCategory, $"Category '{id}' not found.");

       return aCategory!;
    }

    public Task Update(Category anAggregate, CancellationToken aCancellationToken)
    {
        return Task.FromResult(_categories.Update(anAggregate));
    }

    public Task Delete(Category anAggregate, CancellationToken aCancellationToken)
    {
        return Task.FromResult(_categories.Remove(anAggregate));
    }

    public async Task<SearchRepositoryResponse<Category>> Search(SearchRepositoryRequest request, CancellationToken cancellationToken)
    {
        var toSkip = (request.Page - 1) * request.PerPage;
        var query = _categories.AsNoTracking();
        if (!String.IsNullOrWhiteSpace(request.Search))
            query = query.Where(x => x.Name.Contains(request.Search));

        var total = await query.CountAsync();
        var items = await query.AsNoTracking().Skip(toSkip).Take(request.PerPage).ToListAsync();

        return new(request.Page, request.PerPage, total, items);
    }


}
