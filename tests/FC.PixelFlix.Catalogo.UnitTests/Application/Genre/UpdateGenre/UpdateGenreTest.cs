﻿using FC.Pixelflix.Catalogo.Application.Exceptions;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre.Dto;
using FC.Pixelflix.Catalogo.Domain.Exceptions;
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

    [Fact(DisplayName = nameof(GivenAUpdateGenreCommand_whenMissingGenre_shouldThrownNotFound))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    public async Task GivenAUpdateGenreCommand_whenMissingGenre_shouldThrownNotFound()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        
        var aGuid = Guid.NewGuid();

        genreRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Genre {aGuid} not found."));
        
        var useCase = new UseCase.UpdateGenre(categoryRepositoryMock.Object, genreRepositoryMock.Object, unitOfWorkMock.Object);
        
        var input = new UpdateGenreRequest(aGuid, _fixture.GetValidGenreName(), _fixture.GetRandomIsActive());
        
        var action = async() => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre {aGuid} not found.");
    }
    
    [Theory(DisplayName = nameof(GivenAValidUpdateCommand_whenNameIsInvalid_shouldThrownEntityValidationException))]
    [Trait("Application", "UpdateGenre - Use Cases")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GivenAValidUpdateCommand_whenNameIsInvalid_shouldThrownEntityValidationException(string name)
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        
        var aGenre = _fixture.GetValidGenre();
        var newIsActive = !aGenre.IsActive;

        genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(id=>id == aGenre.Id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(aGenre);
        
        var useCase = new UseCase.UpdateGenre(categoryRepositoryMock.Object, genreRepositoryMock.Object, unitOfWorkMock.Object);
        
        var input = new UpdateGenreRequest(aGenre.Id, name, newIsActive);
        
        var action = async() => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<EntityValidationException>().WithMessage($"Name should not be empty or null");
    }

}