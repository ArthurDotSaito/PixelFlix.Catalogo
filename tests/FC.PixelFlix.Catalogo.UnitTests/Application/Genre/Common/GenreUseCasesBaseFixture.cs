using FC.PixelFlix.Catalogo.UnitTests.Common;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.Common;

public class GenreUseCasesBaseFixture : BaseFixture
{
    public string GetValidGenreName()
    {
        return Faker.Commerce.Categories(1)[0];
    }
}