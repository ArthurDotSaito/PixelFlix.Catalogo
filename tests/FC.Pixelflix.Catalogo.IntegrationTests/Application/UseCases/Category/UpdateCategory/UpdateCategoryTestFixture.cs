using FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
using FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestFixtureCollection))]
public class UpdateCategoryTestFixtureCollection: ICollectionFixture<UpdateCategoryTestFixture> { }

public class UpdateCategoryTestFixture : CategoryUseCaseBaseFixture
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