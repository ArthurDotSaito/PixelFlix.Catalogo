using FC.Pixelflix.Catalogo.Infra.Data.EF;
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
        var dbCategory = await assertsDbContext.Categories.FindAsync(aGenre.Id);

        //Then
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(aGenre.Name);
        dbCategory.IsActive.Should().Be(aGenre.IsActive);
        dbCategory.CreatedAt.Should().Be(aGenre.CreatedAt);
        
        var genresCategoryRelation = await assertsDbContext.GenresCategories.Where(gc => gc.GenreId == aGenre.Id).ToListAsync();
        
        genresCategoryRelation.Should().HaveCount(categories.Count);
        genresCategoryRelation.ForEach(relation =>
        {
            var expectedCategory = categories.FirstOrDefault(c => c.Id == relation.CategoryId);
            expectedCategory.Should().NotBeNull();
        });
    }
}