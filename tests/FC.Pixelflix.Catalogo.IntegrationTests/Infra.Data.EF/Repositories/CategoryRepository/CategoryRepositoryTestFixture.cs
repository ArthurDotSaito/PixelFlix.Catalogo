﻿using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.IntegrationTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[CollectionDefinition(nameof(CategoryRepositoryTestFixtureCollection))]
public class CategoryRepositoryTestFixtureCollection : ICollectionFixture<CategoryRepositoryTestFixture> { }
    
public class CategoryRepositoryTestFixture : BaseFixture
{
    public PixelFlixCatalogDbContext CreateDbContext()
    {
        var dbContext = new PixelFlixCatalogDbContext(
            new DbContextOptionsBuilder<PixelFlixCatalogDbContext>()
            .UseInMemoryDatabase("integration-tests-db")
            .Options
        );
    }

    public string GetValidCategoryName()
    {
        var aCategoryName = "";
        while (aCategoryName.Length < 3)
        {
            aCategoryName = Faker.Commerce.Categories(1)[0];
        }
        if (aCategoryName.Length > 255)
        {
            aCategoryName = aCategoryName[..254];
        }

        return aCategoryName;
    }

    public string GetValidCategoryDescription()
    {
        var aCategoryDescription = Faker.Commerce.ProductDescription();
        if (aCategoryDescription.Length > 10000)
        {
            aCategoryDescription = aCategoryDescription[..10000];
        }

        return aCategoryDescription;
    }

    public bool GetRandomIsActive()
    {
        return new Random().NextDouble() < 0.5;
    }

    public Category GetValidCategory()
    {
        var validData = new
        {
            Name = GetValidCategoryName(),
            Description = GetValidCategoryDescription(),
            IsActive = GetRandomIsActive()
        };

        var category = new Category(validData.Name, validData.Description, validData.IsActive);
        return category;
    }
}