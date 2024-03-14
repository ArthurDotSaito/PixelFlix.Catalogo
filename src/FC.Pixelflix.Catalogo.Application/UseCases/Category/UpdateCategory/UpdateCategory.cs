
using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.Domain.Repository;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
public class UpdateCategory : IUpdateCategory
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        categoryRepository = _categoryRepository;
        unitOfWork = _unitOfWork;
    }

    public Task<CategoryModelResponse> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
