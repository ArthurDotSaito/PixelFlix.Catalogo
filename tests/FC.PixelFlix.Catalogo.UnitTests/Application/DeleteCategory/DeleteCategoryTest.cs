using UseCases = FC.Pixelflix.Catalogo.Application.UseCases.Category.DeleteCategory;
using Moq;
using Xunit;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.DeleteCategory;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName ="")]
    [Trait("Appliation", "DeleteCategory - Use Cases")]
    public async Task GivenAValidId_whenCallsDeleteCategory_shouldBeOk()
    {
        //given
        var aRepository = _fixture.GetRepositoryMock();
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
}
