using FC.Pixelflix.Catalogo.Application.Exceptions;
using FC.Pixelflix.Catalogo.Infra.Data.EF;
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
    
    [Fact(DisplayName = "CategoryRepository Integration GET test - Not Found")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenAGetCommand_whenTheresNoGenre_shouldThrowAError()
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
        var ARandomId = Guid.NewGuid();
        
        //When
        var action = async () => await genreRepository.Get(ARandomId, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre '{ARandomId}' was not found");
    }
    
    [Fact(DisplayName = "CategoryRepository Integration DELETE test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenADeleteCommand_whenGenresExist_shouldDeleteAGenre()
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
        var repositoryDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(repositoryDbContext);
    
        //When
        await genreRepository.Delete(aGenre, CancellationToken.None);
        await repositoryDbContext.SaveChangesAsync();

        var assertsDbContext = _fixture.CreateDbContext(true);
        var dbGenre = await assertsDbContext.Genres.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == aGenre.Id);

        //Then
        dbGenre.Should().BeNull();
        
        var genresCategoryRelation = await assertsDbContext.GenresCategories.Where(gc => gc.GenreId == aGenre.Id).ToListAsync();

        genresCategoryRelation.Should().HaveCount(0);
    }
    
    [Fact(DisplayName = "CategoryRepository Integration UPDATE test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenAUpdateCommand_whenGenresExist_shouldUpdateAGenre()
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
        
        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);
        aGenre.Update(_fixture.GetValidGenreName());
        if(aGenre.IsActive) aGenre.Deactivate();
        else aGenre.Activate();
        
        //When
        await genreRepository.Update(aGenre, CancellationToken.None);
        await actDbContext.SaveChangesAsync();

        var assertsDbContext = _fixture.CreateDbContext(true);
        var dbGenre = await assertsDbContext.Genres.FindAsync(aGenre.Id);

        //Then
        dbGenre.Should().NotBeNull();
        dbGenre!.Name.Should().Be(aGenre.Name);
        dbGenre.IsActive.Should().Be(aGenre.IsActive);
        dbGenre.CreatedAt.Should().Be(aGenre.CreatedAt);
        
        var genresCategoryRelation = await assertsDbContext.GenresCategories.Where(gc => gc.GenreId == aGenre.Id).ToListAsync();
        
        genresCategoryRelation.Should().HaveCount(categories.Count);
        genresCategoryRelation.ForEach(relation =>
        {
            var expectedCategory = categories.FirstOrDefault(c => c.Id == relation.CategoryId);
            expectedCategory.Should().NotBeNull();
        });
    }
}