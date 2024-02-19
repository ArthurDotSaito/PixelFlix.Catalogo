using FC.PixelFlix.Catalogo.UnitTests.Common;
using Xunit;
using DomainEntity = FC.Pixelflix.Catalogo.Domain.Entities;


namespace FC.PixelFlix.Catalogo.UnitTests.Domain.Entities.Category;

public class CategoryTestFixture : BaseFixture
{

    public CategoryTestFixture() : base() { }

    public string GetValidCategoryName()
    {
        var aCategoryName = "";
        while (aCategoryName.Length < 3)
        {
            aCategoryName = Faker.Commerce.Categories(1)[0];
        }
        if(aCategoryName.Length > 255) {
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
    public DomainEntity.Category GetValidCategory()
    {
        var validData = new
        {
            Name = GetValidCategoryName(),
            Description = GetValidCategoryDescription()
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, true);
        return category;
    }
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>{}