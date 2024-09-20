using FC.PixelFlix.Catalogo.UnitTests.Application.Genre.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.GetGenre;

[CollectionDefinition(nameof(GetGenreTestFixture))]
public class GetGenreTestFixtureCollection() : ICollectionFixture<GetGenreTestFixture>{}

public class GetGenreTestFixture : GenreUseCasesBaseFixture
{
    
}