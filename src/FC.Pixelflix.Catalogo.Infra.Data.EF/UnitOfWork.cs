using FC.Pixelflix.Catalogo.Application.Interfaces;

namespace FC.Pixelflix.Catalogo.Infra.Data.EF;

public class UnitOfWork : IUnitOfWork
{
    private readonly PixelflixCatalogDbContext _dbContext;
    
    public UnitOfWork(PixelflixCatalogDbContext aDbContext)
    {
        _dbContext = aDbContext;
    }
    public Task Commit(CancellationToken aCancellationToken)
    {
        return _dbContext.SaveChangesAsync(aCancellationToken);
    }

    public Task Rollback(CancellationToken aCancellationToken)
    {
        return Task.CompletedTask;
    }
}