using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;

namespace FC.Pixelflix.Catalogo.Application.Common;
public abstract class PaginatedListRequest
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public string? Search {  get; set; }
    public string? Sort { get; set; }
    public SearchOrder Dir { get; set; }

    public PaginatedListRequest(int page, int perPage, string? search, string? sort, SearchOrder dir)
    {
        Page = page;
        PerPage = perPage;
        if(search is not null) Search = search;
        if(sort is not null) Sort = sort;
        Dir = dir;
    }

    public SearchRepositoryRequest ToSearchRepositoryRequest()
    {
        return new SearchRepositoryRequest(Page, PerPage, Search, Sort, Dir);
    }

}
