﻿using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.Repository;
using FC.PixelFlix.Catalogo.UnitTests.Common;
using Moq;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> { }

public class CreateCategoryTestFixture : BaseFixture
{
    public CreateCategoryTestFixture() : base() { }

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

    public Boolean GetRandomIsActive()
    {
       return (new Random()).NextDouble() < 0.5;
    }

    public CreateCategoryRequest GetValidInput()
    {
        return new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomIsActive());
    }

    public Category GetAValidCategory()
    {
        return new Category(GetValidCategoryName(), GetValidCategoryDescription());
    }

    public CreateCategoryRequest GetInvalidShortNameInput()
    {
        var invalidInputShortName = GetValidInput();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
        return invalidInputShortName;
    }

    public CreateCategoryRequest GetInvalidLongNameInput()
    {
        var invalidInputLongName = GetValidInput();
        var longName = Faker.Commerce.ProductName(); ;
        while (longName.Length < 255)
        {
            longName = $"{longName}{Faker.Commerce.ProductName()}";
        }
        invalidInputLongName.Name = longName;
        return invalidInputLongName;
    }

    public CreateCategoryRequest GetNullDescription()
    {
        var invalidInputDescriptionNull = GetValidInput();
        invalidInputDescriptionNull.Description = null!;
        return invalidInputDescriptionNull;
    }

    public CreateCategoryRequest GetInvalidLongDescription()
    {
        var invalidInputLongDescription = GetValidInput();
        var longDescription = Faker.Commerce.ProductDescription(); ;
        while (longDescription.Length < 10000)
        {
            longDescription = $"{longDescription}{Faker.Commerce.ProductDescription()}";
        }
        invalidInputLongDescription.Description = longDescription;
        return invalidInputLongDescription;
    }

    public Mock<ICategoryRepository> GetMockRepository() => new Mock<ICategoryRepository>();
    public Mock<IUnitOfWork> GetMockUnitOfWork() => new Mock<IUnitOfWork>();
}
