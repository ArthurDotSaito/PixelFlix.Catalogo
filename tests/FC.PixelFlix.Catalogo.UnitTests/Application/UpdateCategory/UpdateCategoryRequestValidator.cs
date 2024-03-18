using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using FluentAssertions;
using Xunit;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.UpdateCategory;
[Collection(nameof(UpdateCategoryTestCollection))]
public class UpdateCategoryRequestValidator
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryRequestValidator(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(givenAEmptyGuid_whenCallsUpdateCategory_thenReturnAError))]
    [Trait("Application", "UpdateCategoryTestValidator - UseCases")]

    public void givenAEmptyGuid_whenCallsUpdateCategory_thenReturnAError()
    {
        //given
        var request = _fixture.GetValidRequest(Guid.Empty);

        var validator = new UpdateCategoryRequestValidation();

        //when
        var validationResponse = validator.Validate(request);

        //then
        validationResponse.Should().NotBeNull();
        validationResponse.IsValid.Should().BeFalse();
        validationResponse.Errors.Should().HaveCount(1);
    }

    [Fact(DisplayName = nameof(givenValidRequest_whenCallsUpdateCategory_thenShouldValidate))]
    [Trait("Application", "UpdateCategoryTestValidator - UseCases")]

    public void givenValidRequest_whenCallsUpdateCategory_thenShouldValidate()
    {
        //given
        var request = _fixture.GetValidRequest();

        var validator = new UpdateCategoryRequestValidation();

        //when
        var validationResponse = validator.Validate(request);

        //then
        validationResponse.Should().NotBeNull();
        validationResponse.IsValid.Should().BeTrue();
        validationResponse.Errors.Should().HaveCount(0);
    }
}
