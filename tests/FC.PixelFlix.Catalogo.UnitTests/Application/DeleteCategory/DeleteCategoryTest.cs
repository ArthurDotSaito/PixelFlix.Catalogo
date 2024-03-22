using UseCases = FC.Pixelflix.Catalogo.Application.UseCases.Category.DeleteCategory;
using Moq;
using Xunit;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.DeleteCategory;
using FC.Pixelflix.Catalogo.Application.Exceptions;
using FluentAssertions;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.DeleteCategory;

[Collection(nameof(DeleteCategoryFixtureCollection))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GivenAValidId_whenCallsDeleteCategory_shouldBeOk))]
    [Trait("Appliation", "DeleteCategory - Use Cases")]
    public async Task GivenAValidId_whenCallsDeleteCategory_shouldBeOk()
    {
        //given
        var aRepository = _fixture.GetMockRepository();
        var aUnitOfWork = _fixture.GetMockUnitOfWork();

        var aValidCategory = _fixture.GetValidCategory();

        aRepository.Setup(category => category.Get(aValidCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(aValidCategory);

        var request = new DeleteCategoryRequest(aValidCategory.Id);

        var useCase = new UseCases.DeleteCategory(aRepository.Object, aUnitOfWork.Object);
        
        //when
        await useCase.Handle(request, CancellationToken.None);

        //then
        aRepository.Verify(category => category.Get(aValidCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
        aRepository.Verify(category => category.Delete(aValidCategory, It.IsAny<CancellationToken>()), Times.Once);
        aUnitOfWork.Verify(category => category.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(GivenAInexistentCategory_whenCallsDeleteCategory_shouldThrowNotFound))]
    [Trait("Appliation", "DeleteCategory - Use Cases")]
    public async Task GivenAInexistentCategory_whenCallsDeleteCategory_shouldThrowNotFound()
    {
        //given
        var aRepository = _fixture.GetMockRepository();
        var aUnitOfWork = _fixture.GetMockUnitOfWork();

        var aInvalidCategory = _fixture.GetValidCategory();

        aRepository.Setup(category => category.Get(aInvalidCategory.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category '{aInvalidCategory.Id} not found"));

        var request = new DeleteCategoryRequest(aInvalidCategory.Id);

        var useCase = new UseCases.DeleteCategory(aRepository.Object, aUnitOfWork.Object);

        //when
        var aTask = async() => await useCase.Handle(request, CancellationToken.None);

        //then
        await aTask.Should().ThrowAsync<NotFoundException>();
        aRepository.Verify(category => category.Get(aInvalidCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
