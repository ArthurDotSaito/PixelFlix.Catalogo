using FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre.Dto;
using FluentAssertions;
using Moq;
using Xunit;
using GenreDomain = FC.Pixelflix.Catalogo.Domain.Entities.Genre;
using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre;

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
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task GivenAValidCommand_whenCallsUpdateGenre_should()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        
        var aGenre = _fixture.GetValidGenre();
        var newName = _fixture.GetValidGenreName();
        var newIsActive = !aGenre.IsActive;

        genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(id=>id == aGenre.Id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(aGenre);
        
        var useCase = new UseCase.UpdateGenre(categoryRepositoryMock.Object, genreRepositoryMock.Object, unitOfWorkMock.Object);
        
        var input = new UpdateGenreRequest(aGenre.Id, newName, newIsActive);
        
        var output = await useCase.Handle(input, CancellationToken.None);
        
        output.Should().NotBeNull();
        output.Id.Should().Be(aGenre.Id);
        output.Name.Should().Be(newName);
        output.IsActive.Should().Be(newIsActive);
        output.CreatedAt.Should().BeSameDateAs(aGenre.CreatedAt);
        
        genreRepositoryMock.Verify(x=>x.Update(It.Is<GenreDomain>(e=>e.Id == aGenre.Id),
            It.IsAny<CancellationToken>()), Times.Once);
        
        unitOfWorkMock.Verify(x=>x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

}