using DomainEntity = FC.Pixelflix.Catalogo.Domain.Entities;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
public class CategoryModelResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public CategoryModelResponse(Guid id, string name, string description, bool isActive, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public static CategoryModelResponse FromCategory(DomainEntity.Category aCategory) =>
         new CategoryModelResponse(aCategory.Id, aCategory.Name, aCategory.Description, aCategory.IsActive, aCategory.CreatedAt);
}
