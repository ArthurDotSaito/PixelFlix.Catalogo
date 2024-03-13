
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
    public void GivenAValidId_whenCallsUpdateCategory_shouldReturnACategory()
    {

    }
}
