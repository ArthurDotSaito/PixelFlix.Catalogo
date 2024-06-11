using FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
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
        var repository = new CategoryRepository(dbContext);
        var request = new GetCategoryRequest(aCategory.Id);
        var useCase = new UseCase.GetCategory(aRepository.Object);

        //when
        var response = await useCase.Handle(request, CancellationToken.None);

        //then
        aRepository.Verify(a => a.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once);

        response.Should().NotBeNull();
        response.Id.Should().Be(aCategory.Id);
        response.Name.Should().Be(aCategory.Name);
        response.Description.Should().Be(aCategory.Description);
        response.IsActive.Should().Be(aCategory.IsActive);
        response.CreatedAt.Should().Be(aCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(GivenValidId_whenCallsGetCategoryWhichDoesntExist_shouldReturnNotFound))]
    [Trait("Application", "GetCategory - useCases")]
    public async Task GivenValidId_whenCallsGetCategoryWhichDoesntExist_shouldReturnNotFound()
    {
        //given
        var aRepository = _fixture.GetMockRepository();
        var aGuid = Guid.NewGuid();

        aRepository.Setup(category => category.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category '{aGuid}' was not found"));
        var request = new GetCategoryRequest(aGuid);
        var useCase = new UseCase.GetCategory(aRepository.Object);

        //when
        var aTask = async () => await useCase.Handle(request, CancellationToken.None);

        //then
        await aTask.Should().ThrowAsync<NotFoundException>();
        aRepository.Verify(a => a.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
}