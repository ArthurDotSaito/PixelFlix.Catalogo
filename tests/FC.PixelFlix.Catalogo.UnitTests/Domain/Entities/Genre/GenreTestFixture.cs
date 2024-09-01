using FC.PixelFlix.Catalogo.UnitTests.Common;
using Xunit;
using DomainGenre = FC.Pixelflix.Catalogo.Domain.Entities.Genre;

namespace FC.PixelFlix.Catalogo.UnitTests.Domain.Entities.Genre;

[CollectionDefinition(nameof(GenreTestFixture))]
public class GenreTestFixtureCollection: ICollectionFixture<GenreTestFixture>{}

public class GenreTestFixture : BaseFixture
{
    public string GetValidName()
    {
        return Faker.Commerce.Categories(1)[0];
    }
    
    public DomainGenre GetAValidGenre(bool isActive = true, List<Guid>? categoriesIdsList = null)
    {
        var genreName = GetValidName();
        var genre = new DomainGenre(genreName, isActive);
        if (categoriesIdsList is not null)
        {
            foreach (var categoryId in categoriesIdsList)
            {
                genre.AddCategory(categoryId);
            }
        }

        return genre;
    }
    
}