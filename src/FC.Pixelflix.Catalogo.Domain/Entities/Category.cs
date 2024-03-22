using FC.Pixelflix.Catalogo.Domain.SeedWork;
using FC.Pixelflix.Catalogo.Domain.Validation;

namespace FC.Pixelflix.Catalogo.Domain.Entities;
public class Category : AggretateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description, bool isActive = true) : base()
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        DomainValidation.NotNullOrEmptyValidation(Name, nameof(Name));

        DomainValidation.MinLengthValidation(Name, 3, nameof(Name));

        DomainValidation.MaxLengthValidation(Name, 255, nameof(Name));

        DomainValidation.NotNullValidation(Description, nameof(Description));

        DomainValidation.MaxLengthValidation(Description, 10000, nameof(Description));
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

    public void Update(string name, string? description = null)
    {
        Name = name; 
        Description = description ?? Description;

        Validate();
    }
}
