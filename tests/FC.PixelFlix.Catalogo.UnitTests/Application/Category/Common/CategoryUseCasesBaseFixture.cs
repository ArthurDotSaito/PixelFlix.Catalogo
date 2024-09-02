using FC.Pixelflix.Catalogo.Application.Interfaces;
using CategoryClass = FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.Repository;
using FC.PixelFlix.Catalogo.UnitTests.Common;
using Moq;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Category.Common;
public abstract class CategoryUseCasesBaseFixture : BaseFixture
{
    public Mock<ICategoryRepository> GetMockRepository() => new Mock<ICategoryRepository>();
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

    public CategoryClass.Category GetValidCategory()
    {
        var validData = new
        {
            Name = GetValidCategoryName(),
            Description = GetValidCategoryDescription(),
            IsActive = GetRandomIsActive()
        };

        var category = new CategoryClass.Category(validData.Name, validData.Description, validData.IsActive);
        return category;
    }
}
