using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.SeedWork;

namespace FC.Pixelflix.Catalogo.Domain.Repository;

public class IGenreRepository : IGenericRepository<Genre>
{
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
}