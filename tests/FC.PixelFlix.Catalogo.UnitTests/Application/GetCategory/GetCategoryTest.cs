using FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory.Dto;
using FluentAssertions;
using Moq;
using Xunit;
using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryFixtureCollection))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GivenAValidId_whenCallsGetCategory_shouldReturnACategory))]
    [Trait("Application", "GetCategory - useCases")]
    public async Task GivenAValidId_whenCallsGetCategory_shouldReturnACategory()
    {
        //given
        var aRepository = _fixture.GetRepositoryMock();
        var aCategory = _fixture.GetAValidCategory();
        
        aRepository.Setup(category => category.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(aCategory);
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
}
