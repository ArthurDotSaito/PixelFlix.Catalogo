using FC.Pixelflix.Catalogo.Application.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
public class ListCategoriesResponse : PaginatedListResponse<CategoryModelResponse>
{
    public ListCategoriesResponse(int page, int perPage, int total, IReadOnlyList<CategoryModelResponse> items) : base(page, perPage, total, items){}
}
