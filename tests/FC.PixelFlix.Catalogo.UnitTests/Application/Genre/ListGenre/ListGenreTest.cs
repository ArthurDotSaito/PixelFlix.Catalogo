using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.ListGenre;

[Collection(nameof(ListGenreTestFixture))]
public class ListGenreTest
{
    private readonly ListGenreTestFixture _fixture;

    public ListGenreTest(ListGenreTestFixture fixture)
    {
        _fixture = fixture;
    }
}