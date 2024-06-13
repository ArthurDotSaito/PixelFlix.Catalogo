using FC.Pixelflix.Catalogo.Application.UseCases.Category.DeleteCategory;
using FC.Pixelflix.Catalogo.Infra.Data.EF;
using FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using useCase = FC.Pixelflix.Catalogo.Application.UseCases.Category.DeleteCategory;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixtureCollection))]
public class DeleteCategoryTestIt
{
    private readonly DeleteCategoryTestFixture _fixture;
    
    public DeleteCategoryTestIt(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(GivenAValidId_whenCallsDeleteCategory_shouldDeleteCategory))]
    [Trait("Integration/Application ", "DeleteCategory - Use Cases")]
    public async Task GivenAValidId_whenCallsDeleteCategory_shouldDeleteCategory()
    {
        //given
        var dbContext = _fixture.CreateDbContext();
        var aCategory = _fixture.GetValidCategory();
        var categoriesToPersist = _fixture.GetValidCategoryList(10);
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        
        await dbContext.AddRangeAsync(categoriesToPersist);
        var categoryTracking = await dbContext.AddAsync(aCategory);
        await dbContext.SaveChangesAsync();
        
        // Detach the entity to simulate a new context
        categoryTracking.State = EntityState.Detached;

        var aValidCategory = _fixture.GetValidCategory();

        var request = new DeleteCategoryRequest(aCategory.Id);

        var useCase = new useCase.DeleteCategory(repository, unitOfWork);

        //when
        await useCase.Handle(request, CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true);
        var dbCategoryDeleted = await assertDbContext.Categories.FindAsync(aCategory.Id);
        //then
        dbCategoryDeleted.Should().BeNull();
        var dbCategories = await assertDbContext.Categories.ToListAsync();
        dbCategories.Should().HaveCount(categoriesToPersist.Count);
    }
    
}