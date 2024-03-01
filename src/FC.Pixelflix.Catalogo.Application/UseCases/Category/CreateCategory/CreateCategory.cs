using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Dto;
using FC.Pixelflix.Catalogo.Domain.Repository;
using DomainEntity = FC.Pixelflix.Catalogo.Domain.Entities;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory;
public class CreateCategory : ICreateCategory
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;
    public async Task<CreateCategoryOutput> Execute(CreateCategoryInput anInput, CancellationToken aCancellationToken)
    {
        var aCategory = new DomainEntity.Category(anInput.Name, anInput.Description, anInput.IsActive);
        await _categoryRepository.Insert(aCategory, aCancellationToken);

        await _unitOfWork.Commit(aCancellationToken);

        return new CreateCategoryOutput(aCategory.Id, aCategory.Name, aCategory.Description, aCategory.IsActive, aCategory.CreatedAt);
    }
}
