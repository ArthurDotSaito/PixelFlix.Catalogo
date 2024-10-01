using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using FC.PixelFlix.Catalogo.UnitTests.Application.Genre.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.ListGenre;

[CollectionDefinition(nameof(ListGenreTestFixture))]
public class ListGenreTestFixtureCollection():ICollectionFixture<ListGenreTestFixture>{}

public class ListGenreTestFixture : GenreUseCasesBaseFixture
{
    public ListGenreRequest GetValidListGenreRequest()
    {
        var random = new Random();

        return new ListGenreRequest(
            page: random.Next(1, 10),
            perPage: random.Next(1, 10),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(1, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
        );
    }
}