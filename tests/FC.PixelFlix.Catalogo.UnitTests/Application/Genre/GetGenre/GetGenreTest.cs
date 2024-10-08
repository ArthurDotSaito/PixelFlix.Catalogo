﻿using FC.Pixelflix.Catalogo.Application.Exceptions;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.GetGenre.Dto;
using FluentAssertions;
using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Genre.GetGenre;
using Moq;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.GetGenre;

[Collection(nameof(GetGenreTestFixture))]
public class GetGenreTest
{
    private readonly GetGenreTestFixture _fixture;

    public GetGenreTest(GetGenreTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsGetGenre_shouldGetAGenre))]
    [Trait("Application", "GetGenre - Use Cases")]
    public async Task GivenAValidCommand_whenCallsGetGenre_shouldGetAGenre()
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
    
    [Fact(DisplayName = nameof(GivenGetCommand_whenThereIsNoGenre_shouldThrowsNotFound))]
    [Trait("Application", "GetGenre - Use Cases")]
    public async Task GivenGetCommand_whenThereIsNoGenre_shouldThrowsNotFound()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
    
        var aGuid = Guid.NewGuid();
        
        genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(id => id == aGuid), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Genre {aGuid} not found"));
        
        var useCase = new UseCase.GetGenre(genreRepositoryMock.Object);
        
        var input = new GetGenreRequest(aGuid);
        
        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre {aGuid} not found");
        
        genreRepositoryMock.Verify(x=>x.Get(It.Is<Guid>(e=>e== aGuid),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}