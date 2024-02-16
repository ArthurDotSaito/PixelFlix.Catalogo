using FC.Pixelflix.Catalogo.Domain.Exceptions;
using Xunit;
using DomainEntity = FC.Pixelflix.Catalogo.Domain.Entities;


namespace FC.PixelFlix.Catalogo.UnitTests.Domain.Entities.Category;
public class CategoryTest
{
    [Fact(DisplayName = nameof(GivenACategoryNewInstance_WhenIsValidDoesNotExist_ShouldBeOK))]
    [Trait("Domain", "Category - Aggregates")]
    public void GivenACategoryNewInstance_WhenIsValidDoesNotExist_ShouldBeOK()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };
        var dateTimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var dateTimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(GivenACategoryNewInstance_WhenIsValidExist_ShouldBeOK))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void GivenACategoryNewInstance_WhenIsValidExist_ShouldBeOK(bool isActive)
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };
        var dateTimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        var dateTimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(GivenACategoryNewInstance_WhenNamePropertyIsEmptyOrNull_ShouldThrowAnError))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void GivenACategoryNewInstance_WhenNamePropertyIsEmptyOrNull_ShouldThrowAnError(string? name)
    {
        Action action = () => new DomainEntity.Category(name!, "Category Description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(GivenACategoryNewInstance_WhenDescriptionPropertyIsNull_ShouldThrowAnError))]
    [Trait("Domain", "Category - Aggregates")]
    public void GivenACategoryNewInstance_WhenDescriptionPropertyIsNull_ShouldThrowAnError()
    {
        Action action = () => new DomainEntity.Category("Category name", null!);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be empty or null", exception.Message);
    }


    [Theory(DisplayName = nameof(GivenACategoryNewInstance_WhenNameHasLessThan3Characters_ShouldThrowAnError))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("ab")]
    [InlineData("a")]
    public void GivenACategoryNewInstance_WhenNameHasLessThan3Characters_ShouldThrowAnError(string invalidName)
    {
        Action action = () => new DomainEntity.Category(invalidName, "Category Description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at least three characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(GivenACategoryNewInstance_WhenNameHasMoreThan255Characters_ShouldThrowAnError))]
    [Trait("Domain", "Category - Aggregates")]
    public void GivenACategoryNewInstance_WhenNameHasMoreThan255Characters_ShouldThrowAnError()
    {
        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "A").ToArray());
        Action action = () => new DomainEntity.Category(invalidName, "Category Description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less than 255 characters long", exception.Message);
    }
}
