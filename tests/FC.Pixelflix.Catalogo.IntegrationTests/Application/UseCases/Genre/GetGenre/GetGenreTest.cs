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

    [Fact(DisplayName = nameof(GivenAValidId_whenCallsGetGenre_shouldReturnACategory))]
    [Trait("Integration/Application", "GetGenre - useCases")]
    public async Task GivenAValidId_whenCallsGetGenre_shouldReturnACategory()
    {
        var aGenreList = _fixture.GetValidGenreList();
    }
}