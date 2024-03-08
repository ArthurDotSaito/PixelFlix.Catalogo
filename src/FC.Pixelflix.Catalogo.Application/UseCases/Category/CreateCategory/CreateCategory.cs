using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
using FC.Pixelflix.Catalogo.Domain.Repository;
using DomainEntity = FC.Pixelflix.Catalogo.Domain.Entities;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory;
public class CreateCategory : ICreateCategory
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategory(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }
    public async Task<CreateCategoryResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        return await Execute(request, cancellationToken);
    }

    public async Task<CreateCategoryResponse> Execute(CreateCategoryRequest anInput, CancellationToken aCancellationToken)
    {
        var aCategory = new DomainEntity.Category(anInput.Name, anInput.Description, anInput.IsActive);

        await _categoryRepository.Insert(aCategory, aCancellationToken);
        await _unitOfWork.Commit(aCancellationToken);

        return CreateCategoryResponse.FromCategory(aCategory);
    }
}
