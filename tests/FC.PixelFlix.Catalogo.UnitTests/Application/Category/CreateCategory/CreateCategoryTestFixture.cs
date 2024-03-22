using FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
using FC.PixelFlix.Catalogo.UnitTests.Application.Category.Common;
using CategoryClass = FC.Pixelflix.Catalogo.Domain.Entities;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> { }

public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public CreateCategoryTestFixture() : base() { }

    public CreateCategoryRequest GetValidInput()
    {
        return new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomIsActive());
    }

    public CategoryClass.Category GetAValidCategory()
    {
        return new CategoryClass.Category(GetValidCategoryName(), GetValidCategoryDescription());
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

}
