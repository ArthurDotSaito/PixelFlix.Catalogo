﻿using FC.Pixelflix.Catalogo.Application.UseCases.Genre.GetGenre.Dto;
using FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;
using FluentAssertions;
using Xunit;
using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Genre.GetGenre;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Genre.GetGenre;

[Collection(nameof(GetGenreTestFixtureCollection))]
public class GetGenreTest
{
    private readonly GetGenreTestFixture _fixture;
    
    public GetGenreTest(GetGenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GivenAValidId_whenCallsGetGenre_shouldReturnACategory))]
    [Trait("Integration/Application", "GetGenre - useCases")]
    public async Task GivenAValidId_whenCallsGetGenre_shouldReturnACategory()
    {
        var dbcontext = _fixture.CreateDbContext();
        
        var aGenreList = _fixture.GetValidGenreList();
        var expectedGenre = aGenreList[5];
        await dbcontext.Genres.AddRangeAsync(aGenreList);
        await dbcontext.SaveChangesAsync();

        var genreRepository = new GenreRepository(_fixture.CreateDbContext(true));
        var request = new GetGenreRequest(expectedGenre.Id);
        var useCase = new UseCase.GetGenre(genreRepository);
        
        var response = await useCase.Handle(request, CancellationToken.None);
        
        response.Should().NotBeNull();
        response.Id.Should().Be(expectedGenre.Id);
        response.Name.Should().Be(expectedGenre.Name);
        response.IsActive.Should().Be(expectedGenre.IsActive);
        response.CreatedAt.Should().Be(expectedGenre.CreatedAt);
    }
}