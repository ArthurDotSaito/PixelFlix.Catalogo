using FC.PixelFlix.Catalogo.UnitTests.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Domain.Entities.Genre;

[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureCollection: ICollectionFixture<GenreTestFixture>{}

public class GenreTestFixture : BaseFixture
{
    public string GetValidName()
    {
        return Faker.Commerce.Categories(1)[0];
    }
}