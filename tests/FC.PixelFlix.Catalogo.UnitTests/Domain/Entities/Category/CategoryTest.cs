﻿using FC.Pixelflix.Catalogo.Domain.Exceptions;
using FluentAssertions;
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

        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        category.CreatedAt.Should().BeAfter(dateTimeBefore);
        category.CreatedAt.Should().BeBefore(dateTimeAfter);
        category.IsActive.Should().BeTrue();
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
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "A").ToArray());
        Action action = () => new DomainEntity.Category(invalidName, "Category Description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less than 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(GivenACategoryNewInstance_WhenDescriptionHasMoreThan10000Characters_ShouldThrowAnError))]
    [Trait("Domain", "Category - Aggregates")]
    public void GivenACategoryNewInstance_WhenDescriptionHasMoreThan10000Characters_ShouldThrowAnError()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "A").ToArray());
        Action action = () => new DomainEntity.Category("Category Name", invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less than 10000 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(GivenANewCategory_WhenCallsActivate_ShouldActivateTheCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void GivenANewCategory_WhenCallsActivate_ShouldActivateTheCategory()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, false);

        category.Activate();

        Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(GivenANewCategory_WhenCallsDeactivate_ShouldDeactivateTheCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void GivenANewCategory_WhenCallsDeactivate_ShouldDeactivateTheCategory()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, true);

        category.Deactivate();

        Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(GivenANewCategory_WhenCallsUpdate_ShouldUpdateAnCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void GivenANewCategory_WhenCallsUpdate_ShouldUpdateAnCategory()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, true);

        var newData = new
        {
            Name = "New Name",
            Description = "New Description"
        };

        category.Update(newData.Name, newData.Description);

        Assert.Equal(newData.Name, category.Name);
        Assert.Equal(newData.Description, category.Description);
    }

    [Fact(DisplayName = nameof(GivenANewCategory_WhenCallsUpdateOnlyWithNameProp_ShouldUpdateAnCategoryName))]
    [Trait("Domain", "Category - Aggregates")]
    public void GivenANewCategory_WhenCallsUpdateOnlyWithNameProp_ShouldUpdateAnCategoryName()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, true);

        var newData = new
        {
            Name = "New Name",
        };

        category.Update(newData.Name);

        Assert.Equal(newData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
    }

    [Theory(DisplayName = nameof(GivenACategoryUpdate_WhenNamePropertyIsEmptyOrNull_ShouldThrowAnError))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void GivenACategoryUpdate_WhenNamePropertyIsEmptyOrNull_ShouldThrowAnError(string? name)
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, true);

        Action action = () => category.Update(name!, "Category Description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(GivenACategoryUpdate_WhenNameHasLessThan3Characters_ShouldThrowAnError))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("ab")]
    [InlineData("a")]
    public void GivenACategoryUpdate_WhenNameHasLessThan3Characters_ShouldThrowAnError(string invalidName)
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, true);

        Action action = () => category.Update(invalidName);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at least three characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(GivenACategoryUpdate_WhenNameHasMoreThan255Characters_ShouldThrowAnError))]
    [Trait("Domain", "Category - Aggregates")]
    public void GivenACategoryUpdate_WhenNameHasMoreThan255Characters_ShouldThrowAnError()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, true);

        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "A").ToArray());
        Action action = () => category.Update(invalidName);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less than 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(GivenACategoryNewInstance_WhenDescriptionHasMoreThan10000Characters_ShouldThrowAnError))]
    [Trait("Domain", "Category - Aggregates")]
    public void GivenACategoryUpdate_WhenDescriptionHasMoreThan10000Characters_ShouldThrowAnError()
    {
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };
        var category = new DomainEntity.Category(validData.Name, validData.Description, true);

        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "A").ToArray());
        Action action = () => category.Update("Category New Name", invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less than 10000 characters long", exception.Message);
    }
}
