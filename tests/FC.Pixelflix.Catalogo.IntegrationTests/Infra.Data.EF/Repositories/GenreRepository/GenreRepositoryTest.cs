﻿using FC.Pixelflix.Catalogo.Infra.Data.EF;
using FC.Pixelflix.Catalogo.Infra.Data.EF.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Repository = FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Infra.Data.EF.Repositories.GenreRepository;

[Collection(nameof(GenreRepositoryTestFixtureCollection))]
public class GenreRepositoryTest
{
    private readonly GenreRepositoryTestFixture _fixture;

    public GenreRepositoryTest(GenreRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = "CategoryRepository Integration INSERT test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenAValidGenre_whenCallsInsert_shouldPersistAGenre()
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var aGenre = _fixture.GetValidGenre();
        var categories = _fixture.GetValidCategoryList(3);
        
        categories.ForEach(category => aGenre.AddCategory(category.Id));
        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
        
        var genreRepository = new Repository.GenreRepository(dbContext);

        //When
        await genreRepository.Insert(aGenre, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var assertsDbContext = _fixture.CreateDbContext(true);
        var dbGenres = await assertsDbContext.Genres.FindAsync(aGenre.Id);

        //Then
        dbGenres.Should().NotBeNull();
        dbGenres!.Name.Should().Be(aGenre.Name);
        dbGenres.IsActive.Should().Be(aGenre.IsActive);
        dbGenres.CreatedAt.Should().Be(aGenre.CreatedAt);
        
        var genresCategoryRelation = await assertsDbContext.GenresCategories.Where(gc => gc.GenreId == aGenre.Id).ToListAsync();
        
        genresCategoryRelation.Should().HaveCount(categories.Count);
        genresCategoryRelation.ForEach(relation =>
        {
            var expectedCategory = categories.FirstOrDefault(c => c.Id == relation.CategoryId);
            expectedCategory.Should().NotBeNull();
        });
    }
    
    [Fact(DisplayName = "CategoryRepository Integration GET test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenAGetCommand_whenTheresAGenre_shouldReturnAGenre()
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var aGenre = _fixture.GetValidGenre();
        var categories = _fixture.GetValidCategoryList(3);
        
        categories.ForEach(category => aGenre.AddCategory(category.Id));
        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.Genres.AddAsync(aGenre);

        foreach (var categoryId in aGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, aGenre.Id);
            await dbContext.GenresCategories.AddRangeAsync(relation);
        }
        
        await dbContext.SaveChangesAsync();
        
        var genreRepository = new Repository.GenreRepository(_fixture.CreateDbContext(true));

        //When
        var databaseGenre = await genreRepository.Get(aGenre.Id, CancellationToken.None);

        var assertsDbContext = _fixture.CreateDbContext(true);

        //Then
        databaseGenre.Should().NotBeNull();
        databaseGenre!.Name.Should().Be(aGenre.Name);
        databaseGenre.IsActive.Should().Be(aGenre.IsActive);
        databaseGenre.CreatedAt.Should().Be(aGenre.CreatedAt);
        databaseGenre.Categories.Should().HaveCount(categories.Count);

        foreach (var categoryId in databaseGenre.Categories)
        {
            var dbCategory = await assertsDbContext.Categories.FindAsync(categoryId);
            dbCategory.Should().NotBeNull();   
        }
    }
}