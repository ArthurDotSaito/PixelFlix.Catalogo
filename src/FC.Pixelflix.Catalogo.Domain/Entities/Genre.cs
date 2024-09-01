using FC.Pixelflix.Catalogo.Domain.Validation;

namespace FC.Pixelflix.Catalogo.Domain.Entities;

public class Genre
{
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyList<Guid> Categories => _Categories.AsReadOnly();
    
    private List<Guid> _Categories;
    
    public Genre(string name, bool isActive = true)
    {
        Name = name;
        IsActive = isActive;
        CreatedAt = DateTime.Now;
        _Categories = new List<Guid>();
        
        Validate();
    }

    public void Activate()
    {
        IsActive = true;
    }
    
    public void Deactivate()
    {
        IsActive = false;
    }

    public void Update(string name)
    {
        Name = name;
        Validate();
    }
    
    public void AddCategory(Guid categoryId)
    {
        _Categories.Add(categoryId);
        Validate();
    }
    
    public void RemoveCategory(Guid categoryId)
    {
        _Categories.Remove(categoryId);
        Validate();
    }
    
    public void RemoveAllCategories()
    {
        _Categories.Clear();
        Validate();
    }
    
    private void Validate()
    {
        DomainValidation.NotNullOrEmptyValidation(Name, nameof(Name));
    }
}