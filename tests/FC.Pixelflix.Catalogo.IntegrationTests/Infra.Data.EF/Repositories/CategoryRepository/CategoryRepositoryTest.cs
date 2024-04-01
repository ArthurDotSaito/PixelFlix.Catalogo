using FluentAssertions;
using Xunit;
using FC.Pixelflix.Catalogo.Infra.Data.EF;
using Repository = FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;
using FC.Pixelflix.Catalogo.Application.Exceptions;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestFixtureCollection))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture categoryRepositoryTestFixture)
    {
        _fixture = categoryRepositoryTestFixture;
    }

    [Fact(DisplayName = "CategoryRepository Integration INSERT test")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task givenAValidCategory_whenCallsInsert_shouldBeOk()
    {
        //Given
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var aCategory = _fixture.GetValidCategory();

        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        //When
        await aCategoryRepository.Insert(aCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        PixelflixCatalogDbContext aSecondContext = _fixture.CreateDbContext();
        var dbCategory = await aSecondContext.Categories.FindAsync(aCategory.Id);

        //Then
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(aCategory.Name);
        dbCategory.Description.Should().Be(aCategory.Description);
        dbCategory.IsActive.Should().Be(aCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(aCategory.CreatedAt);
    }

    [Fact(DisplayName = "CategoryRepository Integration GET test")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task givenAValidCategory_whenCallsGet_shouldBeOk()
    {
        //Given
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var aCategory = _fixture.GetValidCategory();
        var categoriesList = _fixture.GetValidCategoryList(15);
        categoriesList.Add(aCategory);
        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        //When

        var dbCategory = await aCategoryRepository.Get(aCategory.Id, CancellationToken.None);

        //Then
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(aCategory.Id);
        dbCategory.Name.Should().Be(aCategory.Name);
        dbCategory.Description.Should().Be(aCategory.Description);
        dbCategory.IsActive.Should().Be(aCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(aCategory.CreatedAt);
    }

    [Fact(DisplayName = "CategoryRepository Integration INVALID GET test")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task givenAValidCategory_whenCallsGetWithAIdNotPresent_shouldReturnNotFound()
    {
        //Given
        var anId = Guid.NewGuid();
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categoriesList = _fixture.GetValidCategoryList(15);
        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        //When

        var aTask = async () => await aCategoryRepository.Get(anId, CancellationToken.None);

        //Then
        await aTask.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{anId}' not found.");
    }

    [Fact(DisplayName = "CategoryRepository Integration UPDATE test")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task givenAValidCategory_whenCallsUpdate_shouldBeOk()
    {
        //Given
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var aCategory = _fixture.GetValidCategory();
        var aUpdatedCategory = _fixture.GetValidCategory();
        var categoriesList = _fixture.GetValidCategoryList(15);
        categoriesList.Add(aCategory);
        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        aCategory.Update(aUpdatedCategory.Name, aUpdatedCategory.Description);

        //When
        await aCategoryRepository.Update(aCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        PixelflixCatalogDbContext aSecondContext = _fixture.CreateDbContext();
        var dbCategory = await aSecondContext.Categories.FindAsync(aCategory.Id);

        //Then
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(aCategory.Id);
        dbCategory.Name.Should().Be(aUpdatedCategory.Name);
        dbCategory.Description.Should().Be(aUpdatedCategory.Description);
        dbCategory.IsActive.Should().Be(aCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(aCategory.CreatedAt);
    }

    [Fact(DisplayName = "CategoryRepository Integration DELETE test")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task givenAValidCategory_whenCallsDelete_shouldBeOk()
    {
        //Given
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var aCategory = _fixture.GetValidCategory();
        var aUpdatedCategory = _fixture.GetValidCategory();
        var categoriesList = _fixture.GetValidCategoryList(15);
        categoriesList.Add(aCategory);
        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        aCategory.Update(aUpdatedCategory.Name, aUpdatedCategory.Description);

        //When
        await aCategoryRepository.Update(aCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        PixelflixCatalogDbContext aSecondContext = _fixture.CreateDbContext();
        var dbCategory = await aSecondContext.Categories.FindAsync(aCategory.Id);

        //Then
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(aCategory.Id);
        dbCategory.Name.Should().Be(aUpdatedCategory.Name);
        dbCategory.Description.Should().Be(aUpdatedCategory.Description);
        dbCategory.IsActive.Should().Be(aCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(aCategory.CreatedAt);
    }
}
