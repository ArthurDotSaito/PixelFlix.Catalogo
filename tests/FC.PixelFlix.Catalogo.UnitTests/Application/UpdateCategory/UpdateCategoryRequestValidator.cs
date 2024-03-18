using Microsoft.VisualStudio.TestPlatform.ObjectModel;
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

    [Fact(DisplayName = nameof(givenAEmptyGuid_whenCallsUpdateCategory_thenReturnAException))]
    [Trait("Application", "UpdateCategoryTestValidator - UseCases")]

    public void givenAEmptyGuid_whenCallsUpdateCategory_thenReturnAException()
    {
        //given
        var expectedErrorMessage = "'Id' should not be empty";
        var request = _fixture.GetValidRequest(Guid.Empty);

        var validator = new UpdateCategoryRequestValidator();

        //when
        var validationResponse = validator.Validate(request);

        //then
        validationResponse.Should().NotBeNull();
        validationResponse.IsValid.Should().BeFalse();
        validationResponse.Errors.Should().HaveCount(1);
        validationResponse.Errors[0].ErrorMessage.Should().Be(expectedErrorMessage);
    }
}
