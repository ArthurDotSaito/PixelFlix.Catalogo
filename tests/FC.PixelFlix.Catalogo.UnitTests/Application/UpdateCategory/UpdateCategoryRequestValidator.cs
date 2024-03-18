using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.UpdateCategory;
[Collection(nameof(UpdateCategoryTestCollection))]
public class UpdateCategoryRequestValidator
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryRequestValidator(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }


}
