using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.Repository;
using FC.PixelFlix.Catalogo.UnitTests.Common;
using Moq;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryFixtureCollection))]
public class DeleteCategoryFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture> { };

public class DeleteCategoryTestFixture : BaseFixture
{
    public Mock<ICategoryRepository> GetRepositoryMock() => new();
    public Mock<IUnitOfWork> GetMockUnitOfWork() => new Mock<IUnitOfWork>();


    public string GetValidCategoryName()
    {
        var aCategoryName = "";
        while (aCategoryName.Length < 3)
        {
            aCategoryName = Faker.Commerce.Categories(1)[0];
        }
        if (aCategoryName.Length > 255)
        {
            aCategoryName = aCategoryName[..254];
        }

        return aCategoryName;
    }

    public string GetValidCategoryDescription()
    {
        var aCategoryDescription = Faker.Commerce.ProductDescription();
        if (aCategoryDescription.Length > 10000)
        {
            aCategoryDescription = aCategoryDescription[..10000];
        }

        return aCategoryDescription;
    }
    public Category GetValidCategory()
    {
        var validData = new
        {
            Name = GetValidCategoryName(),
            Description = GetValidCategoryDescription()
        };

        var category = new Category(validData.Name, validData.Description, true);
        return category;
    }
}
