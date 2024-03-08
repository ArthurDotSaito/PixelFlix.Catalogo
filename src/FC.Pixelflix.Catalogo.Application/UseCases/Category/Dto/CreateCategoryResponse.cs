using DomainEntity =  FC.Pixelflix.Catalogo.Domain.Entities;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.Dto;
public  class CreateCategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public CreateCategoryResponse(Guid id, string name, string description, bool isActive, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public static CreateCategoryResponse FromCategory(DomainEntity.Category aCategory) =>
         new CreateCategoryResponse(aCategory.Id, aCategory.Name, aCategory.Description, aCategory.IsActive, aCategory.CreatedAt);
}
