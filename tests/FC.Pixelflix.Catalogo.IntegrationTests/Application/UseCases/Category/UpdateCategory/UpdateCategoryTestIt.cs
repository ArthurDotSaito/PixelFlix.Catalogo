using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
using FC.Pixelflix.Catalogo.Infra.Data.EF;
using FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CategoryDomain = FC.Pixelflix.Catalogo.Domain.Entities;
using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixtureCollection))]
public class UpdateCategoryTestIt
{
    private readonly UpdateCategoryTestFixture _fixture;
    
    public UpdateCategoryTestIt(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Theory(DisplayName = nameof(GivenAValidId_whenCallsUpdateCategory_shouldReturnACategory))]
    [Trait("Integration/Application", "UpdateCategory - UseCases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task GivenAValidId_whenCallsUpdateCategory_shouldReturnACategory(CategoryDomain.Category aCategory, UpdateCategoryRequest request)
    {
        //given
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        await dbContext.AddRangeAsync(_fixture.GetValidCategoryList());
        var categoryTracked = await dbContext.AddAsync(aCategory);
        await dbContext.SaveChangesAsync();
        
        // Detach the entity to simulate a new context
        categoryTracked.State = EntityState.Detached;
        
        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);
        
        //when
        var response = await useCase.Handle(request, CancellationToken.None);
        PixelflixCatalogDbContext aSecondContext = _fixture.CreateDbContext(true);
        var dbCategory = await aSecondContext.Categories.FindAsync(response.Id);
        
        //then
        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be((bool)request.IsActive!);
        
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(request.Name);
        dbCategory.Description.Should().Be(request.Description);
        dbCategory.IsActive.Should().Be((bool)request.IsActive);
        dbCategory.CreatedAt.Should().Be(response.CreatedAt);
    }
}