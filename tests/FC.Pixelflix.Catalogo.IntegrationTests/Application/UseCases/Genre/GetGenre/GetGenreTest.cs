using FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Genre.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Genre.GetGenre;

[Collection(nameof(GetGenreTestFixtureCollection))]
public class GetGenreTest
{
    private readonly GetGenreTestFixture _fixture;
    
    public GetGenreTest(GetGenreTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    
}