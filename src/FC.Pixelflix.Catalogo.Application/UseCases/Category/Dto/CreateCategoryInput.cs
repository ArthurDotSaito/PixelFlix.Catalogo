using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.Dto;
public class CreateCategoryInput : IRequest<CreateCategoryOutput>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public CreateCategoryInput(string name, string? description = null, bool isActive = true)
    {
        Name = name;
        Description = description ?? "";
        IsActive = isActive;
    }
}
