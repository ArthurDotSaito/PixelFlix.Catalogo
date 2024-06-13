using FC.Pixelflix.Catalogo.Application.Exceptions;
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
    
    [Theory(DisplayName = nameof(GivenAValidId_whenCallsUpdateCategory_shouldUpdateACategory))]
    [Trait("Integration/Application", "UpdateCategory - UseCases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task GivenAValidId_whenCallsUpdateCategory_shouldUpdateACategory(CategoryDomain.Category aCategory, UpdateCategoryRequest request)
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
    
    [Theory(DisplayName = nameof(GivenAValidId_whenCallsUpdateCategoryWithoutIsActive_shouldUpdateACategory))]
    [Trait("Integration/Application", "UpdateCategory - UseCases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task GivenAValidId_whenCallsUpdateCategoryWithoutIsActive_shouldUpdateACategory(CategoryDomain.Category aCategory, UpdateCategoryRequest request)
    {
        //given
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var aRequestWithoutIsActive = new UpdateCategoryRequest(request.Id, request.Name, request.Description);

        await dbContext.AddRangeAsync(_fixture.GetValidCategoryList());
        var categoryTracked = await dbContext.AddAsync(aCategory);
        await dbContext.SaveChangesAsync();
        
        // Detach the entity to simulate a new context
        categoryTracked.State = EntityState.Detached;
        
        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);
        
        //when
        CategoryModelResponse response = await useCase.Handle(request, CancellationToken.None);
        PixelflixCatalogDbContext aSecondContext = _fixture.CreateDbContext(true);
        var dbCategory = await aSecondContext.Categories.FindAsync(response.Id);

        //then
        response.Should().NotBeNull();
        response.Name.Should().Be(aRequestWithoutIsActive.Name);
        response.Description.Should().Be(aRequestWithoutIsActive.Description);
        response.IsActive.Should().Be((bool)request.IsActive!);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(request.Name);
        dbCategory.Description.Should().Be(request.Description);
        dbCategory.IsActive.Should().Be((bool)request.IsActive);
        dbCategory.CreatedAt.Should().Be(response.CreatedAt);
    }
    
        
    [Theory(DisplayName = nameof(GivenAValidId_whenCallsUpdateCategoryWithNameOnly_shouldUpdateACategory))]
    [Trait("Integration/Application", "UpdateCategory - UseCases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task GivenAValidId_whenCallsUpdateCategoryWithNameOnly_shouldUpdateACategory(CategoryDomain.Category aCategory, UpdateCategoryRequest request)
    {
        //given
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var aRequestWithOnlyName = new UpdateCategoryRequest(request.Id, request.Name);

        await dbContext.AddRangeAsync(_fixture.GetValidCategoryList());
        var categoryTracked = await dbContext.AddAsync(aCategory);
        await dbContext.SaveChangesAsync();
        
        // Detach the entity to simulate a new context
        categoryTracked.State = EntityState.Detached;
        
        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);
        
        //when
        CategoryModelResponse response = await useCase.Handle(aRequestWithOnlyName, CancellationToken.None);
        PixelflixCatalogDbContext aSecondContext = _fixture.CreateDbContext(true);
        var dbCategory = await aSecondContext.Categories.FindAsync(response.Id);

        //then
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(aRequestWithOnlyName.Name);
        dbCategory.Description.Should().Be(aCategory.Description);
        dbCategory.IsActive.Should().Be(aCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(response.CreatedAt);
        response.Should().NotBeNull();
        response.Name.Should().Be(aRequestWithOnlyName.Name);
        response.Description.Should().Be(aCategory.Description);
        response.IsActive.Should().Be(aCategory.IsActive!);
    }
    
    [Fact(DisplayName = nameof(GivenAInvalidId_whenCallsUpdateCategory_shouldReturnNotFound))]
    [Trait("Integration/Application", "UpdateCategory - Use Cases")]
    public async Task GivenAInvalidId_whenCallsUpdateCategory_shouldReturnNotFound()
    {
        //given
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var aRequest = _fixture.GetValidRequest();

        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);

        //when
        var aTask = async () => await useCase.Handle(aRequest, CancellationToken.None);

        //then
        await aTask.Should().ThrowAsync<NotFoundException>();
    }
}