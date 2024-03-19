namespace FC.Pixelflix.Catalogo.Application.Common;
public abstract class PaginatedListResponse<TResponseItem>
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int Total {  get; set; }
    public IReadOnlyList<TResponseItem> Items { get; set; }
    public PaginatedListResponse(int page, int perPage, int total, IReadOnlyList<TResponseItem> items)
    {
        Page = page;
        PerPage = perPage;
        Total = total;
        Items = items;
    }

}
