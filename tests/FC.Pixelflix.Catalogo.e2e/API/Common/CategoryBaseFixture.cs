using FC.Pixelflix.Catalogo.e2e.Base;
using CategoryDomain = FC.Pixelflix.Catalogo.Domain.Entities.Category; 

namespace FC.Pixelflix.Catalogo.e2e.API.Common;

public class CategoryBaseFixture : BaseFixture
{
    public CategoryPersistence Persistence;

    public CategoryBaseFixture() : base()
    {
        Persistence = new CategoryPersistence(CreateDbContext());
    }
    
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

    public bool GetRandomIsActive()
    {
        return new Random().NextDouble() < 0.5;
    }

    public CategoryDomain GetValidCategory()
    {
        var validData = new
        {
            Name = GetValidCategoryName(),
            Description = GetValidCategoryDescription(),
            IsActive = GetRandomIsActive()
        };

        var category = new CategoryDomain(validData.Name, validData.Description, validData.IsActive);
        return category;
    }

    public List<CategoryDomain> GetValidCategoryList(int length = 10)
    {
        var validData = new
        {
            Name = GetValidCategoryName(),
            Description = GetValidCategoryDescription(),
            IsActive = GetRandomIsActive()
        };

        var categoriesList = Enumerable.Range(0, length).Select(_ => GetValidCategory()).ToList();
        return categoriesList;
    }
}