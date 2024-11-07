using FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Genre.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Genre.GetGenre;


[CollectionDefinition(nameof(GetGenreTestFixtureCollection))]
public class GetGenreTestFixtureCollection: ICollectionFixture<GetGenreTestFixture> { }

public class GetGenreTestFixture : GenreUseCaseBaseFixture
{
    
}