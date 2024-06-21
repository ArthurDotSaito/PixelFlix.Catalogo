using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
public class CreateCategoryRequest : IRequest<CategoryModelResponse>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public CreateCategoryRequest(string name, string? description = null, bool isActive = true)
    {
        Name = name;
        Description = description ?? "";    
        IsActive = isActive;
    }
}
