using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.CreateGenre;

[Collection(nameof(CreateGenreTestFixture))]
public class CreateGenreTest
{
    private readonly CreateGenreTestFixture _fixture;

    public CreateGenreTest(CreateGenreTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsCreateGenre_shouldReturnACategory))]
    [Trait("Application", "CreateGenre - Use Cases")]
    public async Task GivenAValidCommand_whenCallsCreateGenre_shouldReturnACategory()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        
        var useCase = new CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object);

        var input = _fixture.GetValidInput();
    }
}