﻿using FC.Pixelflix.Catalogo.Domain.Validation;

namespace FC.Pixelflix.Catalogo.Domain.Entities;

public class Genre
{
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Genre(string name)
    {
        Name = name;
        IsActive = true;
        CreatedAt = DateTime.Now;
    }
    
    public Genre(string name, bool isActive)
    {
        Name = name;
        IsActive = isActive;
        CreatedAt = DateTime.Now;
        
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
    }
    
    private void Validate()
    {
        DomainValidation.NotNullOrEmptyValidation(Name, nameof(Name));
    }
}