using FluentAssertions;
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
        var dateTimeBefore = DateTime.Now;
        var dateTimeAfterCommand = DateTime.Now.AddSeconds(1);
        
        var useCase = new CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object);

        var input = _fixture.GetValidInput();

        var output = await useCase.Handle(input, CancellationToken.None);
        
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBe(null);
        output.CreatedAt.Should().NotBeSameDateAs(default);
        (output.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (output.CreatedAt <= dateTimeAfterCommand).Should().BeTrue();
    }
}