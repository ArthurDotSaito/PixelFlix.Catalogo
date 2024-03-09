using FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory.Dto;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryRequestTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryRequestTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName ="")]
    [Trait("Application", "GetCategoryRequestTest - UseCases")]
    public void GivenValidId_whenCallsGetCategory_shouldValidateOk()
    {
        //given
        var aValidId = Guid.NewGuid();
        var aValidRequest = new GetCategoryRequest(aValidId);

        var validation = new GetCategoryRequestValidation();

        //when
        var aValidResult = validation.Validate(aValidRequest);

        //then
        aValidResult.Should().NotBeNull();  
        aValidResult.IsValid.Should().BeTrue();
        aValidResult.Error.Should().HaveCount(0);
        
    }
}
