using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
using FC.Pixelflix.Catalogo.Domain.Repository;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.DeleteCategory;
public class DeleteCategory : IDeleteCategory
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategory(ICategoryRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    Task IRequestHandler<DeleteCategoryRequest>.Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<CategoryModelResponse> Execute(CreateCategoryRequest anInput, CancellationToken aCancellationToken)
    {
        throw new NotImplementedException();
    }
}
