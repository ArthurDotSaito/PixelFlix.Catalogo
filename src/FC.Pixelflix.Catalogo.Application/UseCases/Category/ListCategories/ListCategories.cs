
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.Domain.Repository;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
public class ListCategories : IListCategories
{
    private readonly ICategoryRepository _categoryRepository;

    public ListCategories(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ListCategoriesResponse> Handle(ListCategoriesRequest request, CancellationToken cancellationToken)
    {
        var repositoryListResponse = await _categoryRepository.Search(new(request.Page, request.PerPage, request.Search, request.Sort, request.Dir), 
            cancellationToken);

        return new ListCategoriesResponse(
            repositoryListResponse.CurrentPage,
            repositoryListResponse.PerPage,
            repositoryListResponse.Total,
            repositoryListResponse.Items.Select(CategoryModelResponse.FromCategory).ToList());

    }
}
