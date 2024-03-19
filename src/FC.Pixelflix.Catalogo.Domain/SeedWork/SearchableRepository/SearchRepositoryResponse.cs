namespace FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
public class SearchRepositoryResponse<TAggregate> where TAggregate : AggretateRoot
{
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<TAggregate> Items { get; set;}

    public SearchRepositoryResponse(int currentPage, int perPage, int total, IReadOnlyList<TAggregate> items)
    {
        CurrentPage = currentPage;
        PerPage = perPage;
        Total = total;
        Items = items;
    }

}
