using Moq;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryFixtureCollection))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "")]
    [Trait("Application", "GetCategory - useCases")]
    public async Task GivenAValidId_whenCallsGetCategory_shouldReturnACategory()
    {
        //given
        var aRepository = _fixture.GetRepositoryMock();
        var aCategory = _fixture.GetAValidCategory();
        aRepository.Setup(category => category.Get(It.IsAny<Guid>, It.IsAny<CancellationToken>())).ReturnsAsync(aCategory);

        //when
        var input = new GetCategoryRequest()

        //then
    }
}
