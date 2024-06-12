using FC.Pixelflix.Catalogo.Infra.Data.EF;
using FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;
using FluentAssertions;
using Xunit;
using useCases = FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.CreateCategory;

[Collection(nameof(CreateCategoryTestFixtureCollection))]
public class CreateCategoryTestIT
{
    private readonly CreateCategoryTestFixture _fixture;
    public CreateCategoryTestIT(CreateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsCreateCategory_shouldBeOk))]
    [Trait("Integration/Application ", "CreateCategory - Use Cases")]
    public async void GivenAValidCommand_whenCallsCreateCategory_shouldBeOk()
    {
        //given
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new useCases.CreateCategory(unitOfWork, repository);

        var request = _fixture.GetValidRequest();

        //when
        var response = await useCase.Execute(request, CancellationToken.None);

        PixelflixCatalogDbContext aSecondContext = _fixture.CreateDbContext(true);
        var dbCategory = await aSecondContext.Categories.FindAsync(response.Id);
        
        //then
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(request.Name);
        dbCategory.Description.Should().Be(request.Description);
        dbCategory.IsActive.Should().Be(request.IsActive);
        dbCategory.CreatedAt.Should().Be(response.CreatedAt);
        
        response.Should().NotBeNull();
        response.Id.Should().NotBeEmpty();
        response.Id.Should().NotBe(default(Guid));
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be(request.IsActive);
        response.CreatedAt.Should().NotBe(null);
        response.CreatedAt.Should().NotBeSameDateAs(default);
    }
}