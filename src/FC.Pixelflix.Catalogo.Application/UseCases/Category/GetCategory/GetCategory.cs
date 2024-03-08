using FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory.Dto;
using FC.Pixelflix.Catalogo.Domain.Repository;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory;
public class GetCategory : IRequestHandler<GetCategoryRequest, GetCategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategory(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<GetCategoryResponse> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
    {
        return await Execute(request, cancellationToken);
    }

    public async Task<GetCategoryResponse> Execute(GetCategoryRequest anInput, CancellationToken aCancellationToken)
    {
        var aCategory = _categoryRepository.Get(anInput.Id, aCancellationToken);

        return GetCategoryResponse.FromCategory(aCategory);
    }
}
