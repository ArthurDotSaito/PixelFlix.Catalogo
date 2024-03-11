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
}
