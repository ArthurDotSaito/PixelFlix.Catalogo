using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory : IRequestHandler<CreateCategoryRequest, CategoryModelResponse>
{
    public Task<CategoryModelResponse> Execute(CreateCategoryRequest anInput, CancellationToken aCancellationToken);

}
