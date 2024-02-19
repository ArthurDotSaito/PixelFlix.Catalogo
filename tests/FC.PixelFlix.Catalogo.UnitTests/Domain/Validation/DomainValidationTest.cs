
using Bogus;
using FC.Pixelflix.Catalogo.Domain.Exceptions;
using FC.Pixelflix.Catalogo.Domain.Validation;
using FluentAssertions;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Domain.Validation;
public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(GivenANotNullDomainValidation_whenFieldIsNotNull_shouldBeOk))]
    [Trait("Domain", "Domain Validation - Validation" )]
    public void GivenANotNullDomainValidation_whenFieldIsNotNull_shouldBeOk()
    {
        var aValidValue = Faker.Commerce.ProductName();
        Action action = () => DomainValidation.NotNullValidation(aValidValue, "Value");

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(GivenANotNullDomainValidation_whenFieldIsNull_shouldThrowsAnException))]
    [Trait("Domain", "Domain Validation - Validation")]
    public void GivenANotNullDomainValidation_whenFieldIsNull_shouldThrowsAnException()
    {
        string nullField = null;
        Action action = () => DomainValidation.NotNullValidation(nullField, "fieldName");

        action.Should().Throw<EntityValidationException>().WithMessage("fieldName should not be null");
    }

    [Theory(DisplayName = nameof(GivenANotNullOrEmptyDomainValidation_whenFieldIsNullOrEmpty_shouldThrowsAnException))]
    [Trait("Domain", "Domain Validation - Validation")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void GivenANotNullOrEmptyDomainValidation_whenFieldIsNullOrEmpty_shouldThrowsAnException(string? target)
    {
        Action action = () => DomainValidation.NotNullOrEmptyValidation(target, "fieldName");

        action.Should().Throw<EntityValidationException>().WithMessage("fieldName should not be null or empty");
    }

    [Fact(DisplayName = nameof(GivenANotNullOrEmptyDomainValidation_whenFieldNotNullOrEmpty_shouldBeOk))]
    [Trait("Domain", "Domain Validation - Validation")]
    public void GivenANotNullOrEmptyDomainValidation_whenFieldNotNullOrEmpty_shouldBeOk()
    {
        var validTarget = Faker.Commerce.ProductName();

        Action action = () => DomainValidation.NotNullOrEmptyValidation(validTarget, "fieldName");

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(GivenAMinLengthDomainValidation_whenFieldWithLessThanCharacters_shouldThrowsAnException))]
    [Trait("Domain", "Domain Validation - Validation")]
    [MemberData(nameof(GetAValueSmallerThanMinLength), parameters:10)]
    public void GivenAMinLengthDomainValidation_whenFieldWithLessThanCharacters_shouldThrowsAnException(string target, int minLength)
    {
        Action action = () => DomainValidation.MinLengthValidation(target, minLength, "fieldName");

        action.Should().Throw<EntityValidationException>().WithMessage($"fieldName should not be less than {minLength} characters long");
    }

    public static IEnumerable<object[]> GetAValueSmallerThanMinLength(int numberOfTests = 5)
    {
        var fakeValues = new Faker();
        for(int i = 0; i < numberOfTests; i++)
        {
            var generatedValue = fakeValues.Commerce.ProductName();
            var generatedValueGreaterThanMin = generatedValue.Length + (new Random()).Next(1, 20);
            yield return new object[] { generatedValue, generatedValueGreaterThanMin };
        }
    }

    [Theory(DisplayName = nameof(GivenAMinLengthDomainValidation_whenFieldWithMoreThanCharacters_shouldBeOK))]
    [Trait("Domain", "Domain Validation - Validation")]
    [MemberData(nameof(GetAValueGreaterThanMinLength), parameters: 10)]
    public void GivenAMinLengthDomainValidation_whenFieldWithMoreThanCharacters_shouldBeOK(string target, int minLength)
    {
        Action action = () => DomainValidation.MinLengthValidation(target, minLength, "fieldName");

        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetAValueGreaterThanMinLength(int numberOfTests = 5)
    {
        var fakeValues = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var generatedValue = fakeValues.Commerce.ProductName();
            var generatedValueGreaterThanMin = generatedValue.Length - (new Random()).Next(1, 5);
            yield return new object[] { generatedValue, generatedValueGreaterThanMin };
        }
    }

    [Theory(DisplayName = nameof(GivenAMaxLengthDomainValidation_whenFieldWithMoreThanCharacters_shouldThrowsAnException))]
    [Trait("Domain", "Domain Validation - Validation")]
    [MemberData(nameof(GetAValueGreaterThanMaxLength), parameters: 10)]
    public void GivenAMaxLengthDomainValidation_whenFieldWithMoreThanCharacters_shouldThrowsAnException(string target, int maxLength)
    {
        Action action = () => DomainValidation.MaxLengthValidation(target, maxLength, "fieldName");

        action.Should().Throw<EntityValidationException>().WithMessage($"fieldName should not be greater than {maxLength} characters long");
    }

    public static IEnumerable<object[]> GetAValueGreaterThanMaxLength(int numberOfTests = 5)
    {
        var fakeValues = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var generatedValue = fakeValues.Commerce.ProductName();
            var generatedValueGreaterThanMax = generatedValue.Length - (new Random()).Next(1, 20);
            yield return new object[] { generatedValue, generatedValueGreaterThanMax };
        }
    }

    [Theory(DisplayName = nameof(GivenAMaxLengthDomainValidation_whenFieldWithLessThanCharacters_shouldBeOK))]
    [Trait("Domain", "Domain Validation - Validation")]
    [MemberData(nameof(GetAValueSmallerThanMaxLength), parameters: 10)]
    public void GivenAMaxLengthDomainValidation_whenFieldWithLessThanCharacters_shouldBeOK(string target, int maxLength)
    {
        Action action = () => DomainValidation.MaxLengthValidation(target, maxLength, "fieldName");

        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetAValueSmallerThanMaxLength(int numberOfTests = 5)
    {
        var fakeValues = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var generatedValue = fakeValues.Commerce.ProductName();
            var generatedValueGreaterThanMax = generatedValue.Length + (new Random()).Next(1, 20);
            yield return new object[] { generatedValue, generatedValueGreaterThanMax };
        }
    }
}
