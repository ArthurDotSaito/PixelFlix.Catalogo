using FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory.Dto;
using FluentAssertions;
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
        aValidResult.Errors.Should().HaveCount(0);
    }

    [Fact(DisplayName = "")]
    [Trait("Application", "GetCategoryRequestTest - UseCases")]
    public void GivenEmptyId_whenCallsGetCategory_shouldThrowsAnException()
    {
        //given
        var aInvalidId = Guid.Empty;
        var aInvalidRequest = new GetCategoryRequest(aInvalidId);

        var validation = new GetCategoryRequestValidation();

        //when
        var aValidResult = validation.Validate(aInvalidRequest);

        //then
        aValidResult.Should().NotBeNull();
        aValidResult.IsValid.Should().BeFalse();
        aValidResult.Errors.Should().HaveCount(1);
        aValidResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
}
