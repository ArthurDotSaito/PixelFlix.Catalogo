using FC.Pixelflix.Catalogo.Application.UseCases.Category.Dto;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory : IRequestHandler<CreateCategoryRequest, CreateCategoryResponse>
{
    public Task<CreateCategoryResponse> Execute(CreateCategoryRequest anInput, CancellationToken aCancellationToken);

}
