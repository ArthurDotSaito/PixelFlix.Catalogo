namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;

public class GenreModelResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Guid> Categories { get; set; }

    public GenreModelResponse(Guid id, string name, bool isActive, DateTime createdAt, List<Guid> categories)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        Categories = categories;
    }
}