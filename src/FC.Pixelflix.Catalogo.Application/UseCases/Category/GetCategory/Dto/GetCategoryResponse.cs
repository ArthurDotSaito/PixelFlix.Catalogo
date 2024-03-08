using FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
using DomainEntity = FC.Pixelflix.Catalogo.Domain.Entities;


namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory.Dto;
public class GetCategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public GetCategoryResponse(Guid id, string name, string description, bool isActive, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public static GetCategoryResponse FromCategory(DomainEntity.Category aCategory) =>
         new GetCategoryResponse(aCategory.Id, aCategory.Name, aCategory.Description, aCategory.IsActive, aCategory.CreatedAt);
}
