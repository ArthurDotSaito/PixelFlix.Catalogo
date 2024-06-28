using FC.Pixelflix.Catalogo.Application.Common;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
public class ListCategoriesRequest : PaginatedListRequest, IRequest<ListCategoriesResponse>
{
    public ListCategoriesRequest(
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc) : base(page, perPage, search, sort, dir){}
    
    public ListCategoriesRequest() : base(1, 15, "", "", SearchOrder.Asc){}
}
