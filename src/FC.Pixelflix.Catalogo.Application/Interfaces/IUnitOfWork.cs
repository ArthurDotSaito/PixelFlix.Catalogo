namespace FC.Pixelflix.Catalogo.Application.Interfaces;
public interface IUnitOfWork
{
    public Task Commit(CancellationToken aCancellationToken);
    
    public Task Rollback(CancellationToken aCancellationToken);

}
