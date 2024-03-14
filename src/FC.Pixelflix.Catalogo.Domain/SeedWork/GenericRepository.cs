using FC.Pixelflix.Catalogo.Domain.Entities;

namespace FC.Pixelflix.Catalogo.Domain.SeedWork;
public interface IGenericRepository<TAggregate> : IRepository
{
    public Task Insert(TAggregate anAggregate, CancellationToken aCancellationToken);
    public Task <TAggregate> Get(Guid id, CancellationToken aCancellationToken);
    public Task Delete(TAggregate anAggregate, CancellationToken aCancellationToken);
    public Task Update(TAggregate anAggregate, CancellationToken aCancellationToken);
}
