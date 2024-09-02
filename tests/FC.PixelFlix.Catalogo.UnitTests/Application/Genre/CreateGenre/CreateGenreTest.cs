using FluentAssertions;
using Moq;
using Xunit;
using DomainGenre = FC.Pixelflix.Catalogo.Domain.Entities.Genre;

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
        
        genreRepositoryMock.Verify(e => e.Insert(It.IsAny<DomainGenre>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(e => e.Commit(It.IsAny<CancellationToken>()), Times.Once);
        
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