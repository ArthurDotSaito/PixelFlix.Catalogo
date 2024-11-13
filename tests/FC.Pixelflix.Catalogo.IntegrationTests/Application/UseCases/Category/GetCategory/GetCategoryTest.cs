using FC.Pixelflix.Catalogo.Application.Exceptions;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory.Dto;
using FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;
using FluentAssertions;
using Xunit;
using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.GetCategory;

[Collection(nameof(GetCategoryTestFixtureCollection))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;
    
    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(GivenAValidId_whenCallsGetCategory_shouldReturnACategory))]
    [Trait("Integration/Application", "GetCategory - useCases")]
    public async Task GivenAValidId_whenCallsGetCategory_shouldReturnACategory()
    {
        //given
        var dbContext = _fixture.CreateDbContext();
        var aCategory = _fixture.GetValidCategory();

        await dbContext.AddRangeAsync(aCategory);
        await dbContext.SaveChangesAsync();
        var repository = new CategoryRepository(dbContext);
        
        var request = new GetCategoryRequest(aCategory.Id);
        var useCase = new UseCase.GetCategory(repository);

        //when
        var response = await useCase.Handle(request, CancellationToken.None);

        //then
        response.Should().NotBeNull();
        response.Id.Should().Be(aCategory.Id);
        response.Name.Should().Be(aCategory.Name);
        response.Description.Should().Be(aCategory.Description);
        response.IsActive.Should().Be(aCategory.IsActive);
        response.CreatedAt.Should().Be(aCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(GivenValidId_whenCallsGetCategoryWhichDoesntExist_shouldReturnNotFound))]
    [Trait("Integration/Application", "GetCategory - useCases")]
    public async Task GivenValidId_whenCallsGetCategoryWhichDoesntExist_shouldReturnNotFound()
    {
        //given
        var dbContext = _fixture.CreateDbContext();
        var aCategory = _fixture.GetValidCategory();

        await dbContext.AddRangeAsync(aCategory);
        dbContext.SaveChanges();
        var repository = new CategoryRepository(dbContext);
        
        var request = new GetCategoryRequest(Guid.NewGuid());
        var useCase = new UseCase.GetCategory(repository);
        
        //when
        var aTask = async () => await useCase.Handle(request, CancellationToken.None);

        //then
        await aTask.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{request.Id}' not found.");
    }
    
}