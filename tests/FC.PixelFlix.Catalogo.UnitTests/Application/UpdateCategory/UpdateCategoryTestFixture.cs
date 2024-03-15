using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.Repository;
using FC.PixelFlix.Catalogo.UnitTests.Common;
using Moq;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestCollection))]
public class UpdateCategoryTestCollection : ICollectionFixture<UpdateCategoryTestFixture>{ }

public class UpdateCategoryTestFixture : BaseFixture
{
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<IUnitOfWork> GetMockUnitOfWork() => new Mock<IUnitOfWork>();

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

    public Category GetAValidCategory()
    {
        return new Category(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomIsActive());
    }

    public UpdateCategoryRequest GetValidRequest(Guid? anId = null)
    {
        var aName = GetValidCategoryName();
        var aDescription = GetValidCategoryDescription();
        var anIsActive = GetRandomIsActive();

        return new UpdateCategoryRequest(anId ?? Guid.NewGuid(), aName, aDescription, anIsActive);
    }

    public UpdateCategoryRequest GetInvalidShortNameInput()
    {
        var invalidInputShortName = GetValidRequest();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
        return invalidInputShortName;
    }

    public UpdateCategoryRequest GetInvalidLongNameInput()
    {
        var invalidInputLongName = GetValidRequest();
        var longName = Faker.Commerce.ProductName(); ;
        while (longName.Length < 255)
        {
            longName = $"{longName}{Faker.Commerce.ProductName()}";
        }
        invalidInputLongName.Name = longName;
        return invalidInputLongName;
    }

    public UpdateCategoryRequest GetNullDescription()
    {
        var invalidInputDescriptionNull = GetValidRequest();
        invalidInputDescriptionNull.Description = null!;
        return invalidInputDescriptionNull;
    }

    public UpdateCategoryRequest GetInvalidLongDescription()
    {
        var invalidInputLongDescription = GetValidRequest();
        var longDescription = Faker.Commerce.ProductDescription(); ;
        while (longDescription.Length < 10000)
        {
            longDescription = $"{longDescription}{Faker.Commerce.ProductDescription()}";
        }
        invalidInputLongDescription.Description = longDescription;
        return invalidInputLongDescription;
    }
}
