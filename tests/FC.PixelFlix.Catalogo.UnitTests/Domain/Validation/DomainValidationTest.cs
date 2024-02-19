
using Bogus;
using FC.Pixelflix.Catalogo.Domain.Exceptions;
using FC.Pixelflix.Catalogo.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Domain.Validation;
public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(givenANotNullDomainValidation_whenFieldIsNotNull_shouldBeOk))]
    [Trait("Domain", "Domain Validation - Validation" )]
    public void givenANotNullDomainValidation_whenFieldIsNotNull_shouldBeOk()
    {
        var aValidValue = Faker.Commerce.ProductName();
        Action action = () => DomainValidation.NotNullDomainValidation(aValidValue, "Value");

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(givenANotNullDomainValidation_whenFieldIsNull_shouldThrowsAnException))]
    [Trait("Domain", "Domain Validation - Validation")]
    public void givenANotNullDomainValidation_whenFieldIsNull_shouldThrowsAnException()
    {
        string nullField = null;
        Action action = () => DomainValidation.NotNullDomainValidation(nullField, "FieldName");

        action.Should().Throw<EntityValidationException>().WithMessage("FieldName should not be null");
    }
}
