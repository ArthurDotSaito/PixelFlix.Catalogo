using FC.Pixelflix.Catalogo.IntegrationTests.Base;
using GenreDomain =  FC.Pixelflix.Catalogo.Domain.Entities.Genre;

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
}