﻿using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Dto;
using FC.Pixelflix.Catalogo.Domain.Repository;
using FC.PixelFlix.Catalogo.UnitTests.Common;
using FC.PixelFlix.Catalogo.UnitTests.Domain.Entities.Category;
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

    public CreateCategoryInput GetValidInput()
    {
        return new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomIsActive());
    }


    public Mock<ICategoryRepository> GetMockRepository() => new Mock<ICategoryRepository>();
    public Mock<IUnitOfWork> GetMockUnitOfWork() => new Mock<IUnitOfWork>();
}