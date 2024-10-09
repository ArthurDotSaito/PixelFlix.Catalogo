using FC.Pixelflix.Catalogo.IntegrationTests.Base;
using Xunit;
using DomainGenre = FC.Pixelflix.Catalogo.Domain.Entities.Genre;
using DomainCategory = FC.Pixelflix.Catalogo.Domain.Entities.Category;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Infra.Data.EF.Repositories.GenreRepository;

[CollectionDefinition(nameof(GenreRepositoryTestFixtureCollection))]
public class GenreRepositoryTestFixtureCollection : ICollectionFixture<GenreRepositoryTestFixture> { }

public class GenreRepositoryTestFixture: BaseFixture
{
    public string GetValidGenreName()
    {
        return Faker.Commerce.Categories(1)[0];
    }
    
    public DomainGenre GetValidGenre()
    {
        var aName = GetValidGenreName();
        var isActive = GetRandomIsActive();
        return new DomainGenre(aName, isActive);
    }
    public DomainGenre GetValidGenre(bool? isActive = null)
    {
        var aName = GetValidGenreName();
        var active = isActive ?? GetRandomIsActive();
        return new DomainGenre(aName, active);
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
    
    public DomainCategory GetValidCategory()
    {
        var validData = new
        {
            Name = GetValidCategoryName(),
            Description = GetValidCategoryDescription(),
            IsActive = GetRandomIsActive()
        };

        var category = new DomainCategory(validData.Name, validData.Description, validData.IsActive);
        return category;
    }
    
    public List<DomainCategory> GetValidCategoryList(int length = 10)
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

    
    public DomainGenre GetValidGenreWithCategories(bool? isActive = null , List<Guid> categoryIds = null)
    {
        var aName = GetValidGenreName();
        var active = isActive ?? GetRandomIsActive();
        
        var genre = new DomainGenre(aName, active);
        categoryIds?.ForEach(categoryId => genre.AddCategory(categoryId));
        return genre;
    }
    
    public bool GetRandomIsActive()
    {
        return new Random().NextDouble() < 0.5;
    }
}