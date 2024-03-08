using FC.Pixelflix.Catalogo.Application.UseCases.Category.Dto;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CreateCategoryOutput>
{
    public Task<CreateCategoryOutput> Execute(CreateCategoryInput anInput, CancellationToken aCancellationToken);

}
