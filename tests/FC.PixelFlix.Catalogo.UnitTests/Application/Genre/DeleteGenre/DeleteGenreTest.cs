using FC.Pixelflix.Catalogo.Application.Exceptions;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.DeleteGenre.Dto;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.GetGenre.Dto;
using FluentAssertions;
using Moq;
using Xunit;
using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Genre.DeleteGenre;
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
    
    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsDeleteGenre_shouldUpdate))]
    [Trait("Application", "DeleteGenre - Use Cases")]
    public async Task GivenAValidCommand_whenCallsDeleteGenre_shouldUpdate()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        
        var aGenre = _fixture.GetValidGenre();

        genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(id=>id == aGenre.Id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(aGenre);
        
        var useCase = new UseCase.DeleteGenre(genreRepositoryMock.Object, unitOfWorkMock.Object);
        
        var input = new DeleteGenreRequest(aGenre.Id);
        
        await useCase.Handle(input, CancellationToken.None);
        
        genreRepositoryMock.Verify(x => x.Get(It.Is<Guid>(id=>id == aGenre.Id), 
            It.IsAny<CancellationToken>()), Times.Once);
        
        unitOfWorkMock.Verify(x=>x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact(DisplayName = nameof(GivenGetCommand_whenThereIsNoGenre_shouldThrowsNotFound))]
    [Trait("Application", "DeleteGenre - Use Cases")]
    public async Task GivenGetCommand_whenThereIsNoGenre_shouldThrowsNotFound()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
    
        var aGuid = Guid.NewGuid();
        
        genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(id => id == aGuid), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Genre {aGuid} not found"));
        
        var useCase = new UseCase.DeleteGenre(genreRepositoryMock.Object, unitOfWorkMock.Object);
        
        var input = new DeleteGenreRequest(aGuid);
        
        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre {aGuid} not found");
        
        genreRepositoryMock.Verify(x=>x.Get(It.Is<Guid>(e=>e== aGuid),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
