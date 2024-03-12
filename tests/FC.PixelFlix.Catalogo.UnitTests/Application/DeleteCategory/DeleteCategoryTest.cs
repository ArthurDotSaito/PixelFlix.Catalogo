using Moq;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
internal class DeleteCategoryTest
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

        var useCase = new DeleteCategory(aRepository.Object, aUnitOfWork.Object);
        
        //when
        await useCase.Handle(request, CancellationToken.None);

        //then
        aRepository.Verify(category => category.Get(aValidCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
        aRepository.Verify(category => category.Delete(aValidCategory, It.IsAny<CancellationToken>()), Times.Once);
        aUnitOfWork.Verify(category => category.Commit(It.IsAny<CancellationToken>()), Times.Once);


    }
}
