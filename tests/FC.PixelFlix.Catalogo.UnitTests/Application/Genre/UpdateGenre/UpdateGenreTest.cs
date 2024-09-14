using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.UpdateGenre;

[Collection(nameof(UpdateGenreTestFixture))]
public class UpdateGenreTest
{
    private readonly UpdateGenreTestFixture _fixture;

    public UpdateGenreTest(UpdateGenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsUpdateGenre_should))]
    [Trait("Application", "CreateGenre - Use Cases")]
    public async Task GivenAValidCommand_whenCallsUpdateGenre_should()
    {
        
    }

}