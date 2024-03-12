using FC.Pixelflix.Catalogo.Application.Interfaces;
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

    public async Task<Unit> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _repository.Get(request.Id, cancellationToken);
        await _repository.Delete(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return Unit.Value;
    }
}
