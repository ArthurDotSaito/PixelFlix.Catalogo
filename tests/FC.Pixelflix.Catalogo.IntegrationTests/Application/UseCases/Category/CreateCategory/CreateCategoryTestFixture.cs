using FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
using FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixtureCollection))]
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> { };

public class CreateCategoryTestFixture : CategoryUseCaseBaseFixture
{
    public CreateCategoryRequest GetValidRequest()
    {
        var category = GetValidCategory();
        return new CreateCategoryRequest(category.Name, category.Description, category.IsActive);
    }  
    
    public CreateCategoryRequest GetInvalidShortNameInput()
    {
        var invalidInputShortName = GetValidRequest();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
        return invalidInputShortName;
    }

    public CreateCategoryRequest GetInvalidLongNameInput()
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

    public CreateCategoryRequest GetNullDescription()
    {
        var invalidInputDescriptionNull = GetValidRequest();
        invalidInputDescriptionNull.Description = null!;
        return invalidInputDescriptionNull;
    }

    public CreateCategoryRequest GetInvalidLongDescription()
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