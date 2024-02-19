using FC.PixelFlix.Catalogo.UnitTests.Common;
using Xunit;
using DomainEntity = FC.Pixelflix.Catalogo.Domain.Entities;


namespace FC.PixelFlix.Catalogo.UnitTests.Domain.Entities.Category;

public class CategoryTestFixture : BaseFixture
{

    public CategoryTestFixture() : base() { }
    public DomainEntity.Category GetValidCategory()
    {
        var validData = new
        {
            Name = Faker.Commerce.Categories(1)[0],
            Description = Faker.Commerce.ProductDescription()
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, true);
        return category;
    }
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>{}