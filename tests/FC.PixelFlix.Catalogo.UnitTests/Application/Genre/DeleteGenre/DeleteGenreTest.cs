using FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre.Dto;
using FluentAssertions;
using Moq;
using Xunit;
using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre;
using GenreDomain = FC.Pixelflix.Catalogo.Domain.Entities.Genre;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.DeleteGenre;

[Collection(nameof(DeleteGenreTestFixture))]
public class DeleteGenreTest
{
    private readonly DeleteGenreTestFixture _fixture;

    public DeleteGenreTest(DeleteGenreTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsUpdateGenre_shouldUpdate))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task GivenAValidCommand_whenCallsUpdateGenre_shouldUpdate()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        
        var aGenre = _fixture.GetValidGenre();
        var newName = _fixture.GetValidGenreName();
        var newIsActive = !aGenre.IsActive;

        genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(id=>id == aGenre.Id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(aGenre);
        
        var useCase = new UseCase.DeleteGenre(categoryRepositoryMock.Object, genreRepositoryMock.Object, unitOfWorkMock.Object);
        
        var input = new DeleteGenreRequest(aGenre.Id);
        
        await useCase.Handle(input, CancellationToken.None);
        
        genreRepositoryMock.Verify(x => x.Get(It.Is<Guid>(id=>id == aGenre.Id), 
            It.IsAny<CancellationToken>()), Times.Once);
        
        unitOfWorkMock.Verify(x=>x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
}
