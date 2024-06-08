﻿using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using FC.Pixelflix.Catalogo.Infra.Data.EF;
using FC.Pixelflix.Catalogo.IntegrationTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[CollectionDefinition(nameof(CategoryRepositoryTestFixtureCollection))]
public class CategoryRepositoryTestFixtureCollection : ICollectionFixture<CategoryRepositoryTestFixture> { }
    
public class CategoryRepositoryTestFixture : BaseFixture
{
    public PixelflixCatalogDbContext CreateDbContext(bool preserveData = false)
    {
        var dbContext = new PixelflixCatalogDbContext(
            new DbContextOptionsBuilder<PixelflixCatalogDbContext>()
            .UseInMemoryDatabase("integration-tests-db")
            .Options
        );

        if (preserveData == false) dbContext.Database.EnsureDeleted();

        return dbContext;
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

    public List<Category> GetValidCategoryList(int length = 10)
    {
        var validData = new
        {
            Name = GetValidCategoryName(),
            Description = GetValidCategoryDescription(),
            IsActive = GetRandomIsActive()
        };

        var categoriesList = Enumerable.Range(0, length).Select(_ => GetValidCategory()).ToList();
        return categoriesList;
    }

    public List<Category> GetValidCategoryListWithNames(List<string> names)
    {
        return names.Select(name =>
        {
            var category = GetValidCategory();
            category.Update(name);
            return category;
        }).ToList();
    }

    public List<Category> CloneCategoryListListAndOrderIt(List<Category> categories,string orderBy, SearchOrder searchOrder)
    {
        var newCategoriesList = new List<Category>(categories);
        var newCategoriesListEnumerable = (orderBy, searchOrder) switch
        {

            ("name", SearchOrder.Asc) => newCategoriesList.OrderBy(items => items.Name),
            ("name", SearchOrder.Desc) => newCategoriesList.OrderByDescending(items => items.Name),
            _ => newCategoriesList.OrderBy(items => items.Name),
        };

        return newCategoriesListEnumerable.ToList();
    }
}
