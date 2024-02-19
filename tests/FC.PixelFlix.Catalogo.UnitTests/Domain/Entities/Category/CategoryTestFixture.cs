using Xunit;
using DomainEntity = FC.Pixelflix.Catalogo.Domain.Entities;


namespace FC.PixelFlix.Catalogo.UnitTests.Domain.Entities.Category;

public class CategoryTestFixture
{
    public DomainEntity.Category GetValidCategory()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, true);
        return category;
    }
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>{}