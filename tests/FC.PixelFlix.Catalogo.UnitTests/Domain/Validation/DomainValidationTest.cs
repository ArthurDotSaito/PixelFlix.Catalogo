
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
        Action action = () => DomainValidation.NotNullValidation(aValidValue, "Value");

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(givenANotNullDomainValidation_whenFieldIsNull_shouldThrowsAnException))]
    [Trait("Domain", "Domain Validation - Validation")]
    public void givenANotNullDomainValidation_whenFieldIsNull_shouldThrowsAnException()
    {
        string nullField = null;
        Action action = () => DomainValidation.NotNullValidation(nullField, "fieldName");

        action.Should().Throw<EntityValidationException>().WithMessage("fieldName should not be null");
    }

    [Theory(DisplayName = nameof(givenANotNullOrEmptyDomainValidation_whenFieldIsNullOrEmpty_shouldThrowsAnException))]
    [Trait("Domain", "Domain Validation - Validation")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void givenANotNullOrEmptyDomainValidation_whenFieldIsNullOrEmpty_shouldThrowsAnException(string? target)
    {
        Action action = () => DomainValidation.NotNullOrEmptyValidation(target, "fieldName");

        action.Should().Throw<EntityValidationException>().WithMessage("fieldName should not be null or empty");
    }

    [Fact(DisplayName = nameof(givenANotNullOrEmptyDomainValidation_whenFieldNotNullOrEmpty_shouldBeOk))]
    [Trait("Domain", "Domain Validation - Validation")]
    public void givenANotNullOrEmptyDomainValidation_whenFieldNotNullOrEmpty_shouldBeOk()
    {
        var validTarget = Faker.Commerce.ProductName();

        Action action = () => DomainValidation.NotNullOrEmptyValidation(validTarget, "fieldName");

        action.Should().NotThrow();
    }
}
