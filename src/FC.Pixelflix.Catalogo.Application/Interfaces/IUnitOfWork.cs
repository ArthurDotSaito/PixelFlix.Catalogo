namespace FC.Pixelflix.Catalogo.Application.Interfaces;
public interface IUnitOfWork
{
    public Task Commit(CancellationToken aCancellationToken);
}
