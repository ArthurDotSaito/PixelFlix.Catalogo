
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
public class UpdateCategoryRequest:IRequest<CategoryModelResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }

    public UpdateCategoryRequest(Guid id, string name, string? description = null, bool? isActive = null)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
    }

}
