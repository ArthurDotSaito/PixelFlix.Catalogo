﻿using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;

namespace FC.Pixelflix.Catalogo.Application.Common;
public abstract class PaginatedListRequest
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public string Search {  get; set; }
    public string Sort { get; set; }
    public SearchOrder Dir { get; set; }

    public PaginatedListRequest(int page, int perPage, string search, string sort, SearchOrder dir)
    {
        Page = page;
        PerPage = perPage;
        Search = search;
        Sort = sort;
        Dir = dir;
    }

}
