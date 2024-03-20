
using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.Repository;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using FC.PixelFlix.Catalogo.UnitTests.Application.UpdateCategory;
using FC.PixelFlix.Catalogo.UnitTests.Common;
using Moq;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.ListCategories;


[CollectionDefinition(nameof(ListCategoriesTestCollection))]
public class ListCategoriesTestCollection : ICollectionFixture<ListCategoriesTestFixture> { }
public class ListCategoriesTestFixture : BaseFixture
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

    public Boolean GetRandomIsActive()
    {
        return (new Random()).NextDouble() < 0.5;
    }

    public Category GetAValidCategory()
    {
        return new Category(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomIsActive());
    }

    public List<Category> GetValidCategoryList(int length = 10)
    {
        var categoriesList = new List<Category>();
        for (var i = 0; i < length; i++)
        {
            categoriesList.Add(GetAValidCategory());
        }

        return categoriesList;
 
    }

    public ListCategoriesRequest GetValidRequest()
    {
        var random = new Random();

        return new ListCategoriesRequest(
            page: random.Next(1, 10),
            perPage: random.Next(1, 10),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(1, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
       );
    }
}
