
namespace FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
public interface ISearchableRepository<TAggregate> where TAggregate : AggretateRoot
{
    Task<SearchRepositoryResponse<TAggregate>> Search(SearchRepositoryRequest request, CancellationToken cancellationToken);
}
