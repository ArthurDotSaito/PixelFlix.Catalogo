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
    
    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsListGenre_shouldReturnAListOfGenres))]
    [Trait("Application", "ListGenre - Use Cases")]
    public async Task GivenAValidCommand_whenCallsListGenre_shouldReturnAListOfGenres()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();

        var someCategories = _fixture.GenerateRandomCategoryIds(10);
        var aGenre = _fixture.GetValidGenreWithCategories(categoryIds: someCategories);

        genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(id=>id == aGenre.Id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(aGenre);
        
        var useCase = new UseCase.GetGenre(genreRepositoryMock.Object);
        
        var input = new GetGenreRequest(aGenre.Id);
        
        var output = await useCase.Handle(input, CancellationToken.None);
        
        output.Should().NotBeNull();
        output.Id.Should().Be(aGenre.Id);
        output.Name.Should().Be(aGenre.Name);
        output.IsActive.Should().Be(aGenre.IsActive);
        output.CreatedAt.Should().BeSameDateAs(aGenre.CreatedAt);
        output.Categories.Should().HaveCount(someCategories.Count);

        foreach (var expectedId in aGenre.Categories)
        {
            output.Categories.Should().Contain(expectedId);
        }
        
        genreRepositoryMock.Verify(x=>x.Get(It.Is<Guid>(e=>e== aGenre.Id),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}