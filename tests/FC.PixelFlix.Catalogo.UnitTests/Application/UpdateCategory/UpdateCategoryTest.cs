
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Moq;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryTestCollection))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName ="")]
    [Trait("Application", "UpdateCategory - UseCases")]
    public async Task GivenAValidId_whenCallsUpdateCategory_shouldReturnACategory()
    {
        //given
        var aRepository = _fixture.GetRepositoryMock();
        var aUnitOfWork = _fixture.GetMockUnitOfWork();
        var aCategory = _fixture.GetAValidCategory();
        var expectedName = _fixture.GetValidCategoryName();
        var expectedDescription = _fixture.GetValidCategoryDescription();
        var expectedIsActive = !_fixture.GetRandomIsActive();
        var cancellationToken = It.IsAny<CancellationToken>();

        aRepository.Setup(category => category.Get(aCategory.Id, cancellationToken))
            .ReturnsAsync(aCategory);

        var request = new UseCase.UpdateCategoryRequest(aCategory.Id ,expectedName, expectedDescription, expectedIsActive);

        var useCase = new UseCase.UpdateCategory(aRepository.Object, aUnitOfWork.Object);


        //when

        CategoryModelResponse response = await useCase.Handle(request, cancellationToken);

        //then

        response.Should().NotBeNull();
        response.Name.Should().Be(expectedName);
        response.Description.Should().Be(expectedDescription);
        response.IsActive.Should().Be(expectedIsActive);

        aRepository.Verify(category => category.Get(aCategory.Id, It.IsAny<CancellationToken>()), 
            Times.Once);

        aRepository.Verify(category => category.Update(aCategory, It.IsAny<CancellationToken>()),
            Times.Once);

        aUnitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
}
