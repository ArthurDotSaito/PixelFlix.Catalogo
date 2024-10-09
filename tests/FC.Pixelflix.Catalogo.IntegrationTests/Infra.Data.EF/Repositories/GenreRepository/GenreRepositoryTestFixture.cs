using FC.Pixelflix.Catalogo.IntegrationTests.Base;
using Xunit;
using DomainGenre = FC.Pixelflix.Catalogo.Domain.Entities.Genre;

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