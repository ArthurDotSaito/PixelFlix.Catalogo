namespace FC.Pixelflix.Catalogo.Api.ApiModels.Category;

public class UpdateCategoryApiRequest
{
    public UpdateCategoryApiRequest(string name, string? description = null, bool? isActive = null)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    public string Name { get; set; }
    public string? Description { get; set; } 
    public bool? IsActive { get; set; }
}
