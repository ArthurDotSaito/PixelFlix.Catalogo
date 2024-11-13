using FC.Pixelflix.Catalogo.IntegrationTests.Base;
using GenreDomain =  FC.Pixelflix.Catalogo.Domain.Entities.Genre;
using CategoryDomain =  FC.Pixelflix.Catalogo.Domain.Entities.Category;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Genre.Common;

public class GenreUseCaseBaseFixture : BaseFixture
{
    public string GetValidGenreName()
    {
        var aGenreName = "";
        while (aGenreName.Length < 3)
        {
            aGenreName = Faker.Commerce.Categories(1)[0];
        }
        if (aGenreName.Length > 255)
        {
            aGenreName = aGenreName[..254];
        }

        return aGenreName;
    }

    public bool GetRandomIsActive()
    {
        return new Random().NextDouble() < 0.5;
    }

    public GenreDomain GetValidGenre()
    {
        var validData = new
        {
            Name = GetValidGenreName(),
            IsActive = GetRandomIsActive()
        };

        var category = new GenreDomain(validData.Name, validData.IsActive);
        return category;
    }

    public List<GenreDomain> GetValidGenreList(int length = 10)
    {
        var validData = new
        {
            Name = GetValidGenreName(),
            IsActive = GetRandomIsActive()
        };

        var categoriesList = Enumerable.Range(0, length).Select(_ => GetValidGenre()).ToList();
        return categoriesList;
    }
    
    public GenreDomain GetValidGenreWithCategories(bool? isActive = null , List<Guid> categoryIds = null, string name = null)
    {
        var aName = name ?? GetValidGenreName();
        var active = isActive ?? GetRandomIsActive();
        
        var genre = new GenreDomain(aName, active);
        categoryIds?.ForEach(categoryId => genre.AddCategory(categoryId));
        return genre;
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