using FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
using FC.PixelFlix.Catalogo.UnitTests.Application.Category.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestCollection))]
public class UpdateCategoryTestCollection : ICollectionFixture<UpdateCategoryTestFixture> { }

public class UpdateCategoryTestFixture : CategoryUseCasesBaseFixture
{
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
