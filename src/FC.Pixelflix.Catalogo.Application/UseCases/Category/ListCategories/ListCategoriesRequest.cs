using FC.Pixelflix.Catalogo.Application.Common;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
public class ListCategoriesRequest : PaginatedListRequest, IRequest<ListCategoriesResponse>
{
    public ListCategoriesRequest(int page, int perPage, string search, string sort, SearchOrder dir) : base(page, perPage, search, sort, dir){}
}
