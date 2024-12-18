﻿using FC.Pixelflix.Catalogo.Application.Exceptions;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.GetGenre.Dto;
using FC.Pixelflix.Catalogo.Infra.Data.EF.Models;
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

    [Fact(DisplayName = nameof(GivenAValidId_whenCallsGetGenre_shouldReturnAGenre))]
    [Trait("Integration/Application", "GetGenre - useCases")]
    public async Task GivenAValidId_whenCallsGetGenre_shouldReturnAGenre()
    {
        var dbContext = _fixture.CreateDbContext();
        
        var aGenreList = _fixture.GetValidGenreList();
        var expectedGenre = aGenreList[5];
        await dbContext.Genres.AddRangeAsync(aGenreList);
        await dbContext.SaveChangesAsync();

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
    
    [Fact(DisplayName = nameof(GivenAValidId_whenThereIsNoId_shouldThrownNotFound))]
    [Trait("Integration/Application", "GetGenre - useCases")]
    public async Task GivenAValidId_whenThereIsNoId_shouldThrownNotFound()
    {
        var dbContext = _fixture.CreateDbContext();
        var randomId = Guid.NewGuid();
        
        var aGenreList = _fixture.GetValidGenreList();
        var expectedGenre = aGenreList[5];
        await dbContext.Genres.AddRangeAsync(aGenreList);
        await dbContext.SaveChangesAsync();

        var genreRepository = new GenreRepository(_fixture.CreateDbContext(true));
        var request = new GetGenreRequest(randomId);
        var useCase = new UseCase.GetGenre(genreRepository);
        
        var output = async() => await useCase.Handle(request, CancellationToken.None);

        await output.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre '{randomId}' was not found");
    }
    
    [Fact(DisplayName = nameof(GivenAValidId_whenThereIsRelations_shouldReturnAGenreWithCategory))]
    [Trait("Integration/Application", "GetGenre - useCases")]
    public async Task GivenAValidId_whenThereIsRelations_shouldReturnAGenreWithCategory()
    {
        var dbContext = _fixture.CreateDbContext();
        var categoriesList = _fixture.GetValidCategoryList(5);
        var aGenreList = _fixture.GetValidGenreList();
        var expectedGenre = aGenreList[5];
        
        categoriesList.ForEach(category => expectedGenre.AddCategory(category.Id));
        
        await dbContext.Categories.AddRangeAsync(categoriesList);
        await dbContext.Genres.AddRangeAsync(aGenreList);
        await dbContext.GenresCategories.AddRangeAsync(expectedGenre.Categories.Select(categoryId => 
            new GenresCategories(categoryId,expectedGenre.Id)));
        
        await dbContext.SaveChangesAsync();
        
        var genreRepository = new GenreRepository(_fixture.CreateDbContext(true));
        var request = new GetGenreRequest(expectedGenre.Id);
        var useCase = new UseCase.GetGenre(genreRepository);
        
        var response = await useCase.Handle(request, CancellationToken.None);
        
        response.Should().NotBeNull();
        response.Id.Should().Be(expectedGenre.Id);
        response.Name.Should().Be(expectedGenre.Name);
        response.IsActive.Should().Be(expectedGenre.IsActive);
        response.CreatedAt.Should().Be(expectedGenre.CreatedAt);
        response.Categories.Should().NotBeEmpty();
        response.Categories.Should().HaveCount(expectedGenre.Categories.Count);
        
        response.Categories.ToList().ForEach(id => expectedGenre.Categories.Should().Contain(id));
    }
}