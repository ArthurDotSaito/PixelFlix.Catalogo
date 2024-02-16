
using FC.Pixelflix.Catalogo.Domain.Exceptions;

namespace FC.Pixelflix.Catalogo.Domain.Entities;
public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description, bool isActive = true)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        if(String.IsNullOrWhiteSpace(Name))
            throw new EntityValidationException($"{nameof(Name)} should not be empty or null");

        if (Name.Length < 3)
            throw new EntityValidationException($"{nameof(Name)} should be at least three characters long");

        if (Name.Length > 255)
            throw new EntityValidationException($"{nameof(Name)} should be less than 255 characters long");

        if (Description == null)
            throw new EntityValidationException($"{nameof(Description)} should not be empty or null");

        if (Description.Length > 10000)
            throw new EntityValidationException($"{nameof(Description)} should be less than 10000 characters long");
    }

    public void Activate()
    {
        IsActive = true;
        Validate();
    }

    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }
}
