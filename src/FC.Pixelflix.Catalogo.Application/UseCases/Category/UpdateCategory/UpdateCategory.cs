
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
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryModelResponse> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        category.Update(request.Name, request.Description);
        if(request.IsActive != category.IsActive)
        {
            if(request.IsActive) category.Activate();
            else category.Deactivate();
        }

        await _categoryRepository.Update(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CategoryModelResponse.FromCategory(category);
    }
}
