using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestFixtureCollection))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture categoryRepositoryTestFixture)
    {
        _fixture = categoryRepositoryTestFixture;
    }

    [Fact(DisplayName = "Verificar categoria e propriedades no repositório")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task givenAValidCategory_whenCallsInsert_shouldBeOk()
    {
        PixelFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var aCategory = _fixture.GetValidCategory();

        var aCategoryRepository = new CategoryRepository(dbContext);

        await aCategoryRepository.Insert(aCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var dbCategory = await dbContext.Categories.Find(aCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(aCategory.Name);
        dbCategory.Description.Should().Be(aCategory.Description);
        dbCategory.IsActive.Should().Be(aCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(aCategory.CreatedAt);
    }
}
