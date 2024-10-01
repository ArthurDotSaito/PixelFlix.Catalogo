using FC.PixelFlix.Catalogo.UnitTests.Application.Genre.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.ListGenre;

[CollectionDefinition(nameof(ListGenreTestFixture))]
public class ListGenreTestFixtureCollection():ICollectionFixture<ListGenreTestFixture>{}

public class ListGenreTestFixture : GenreUseCasesBaseFixture
{
    
}