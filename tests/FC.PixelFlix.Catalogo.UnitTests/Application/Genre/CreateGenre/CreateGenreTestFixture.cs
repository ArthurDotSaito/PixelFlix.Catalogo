using FC.PixelFlix.Catalogo.UnitTests.Application.Genre.Common;
using FC.PixelFlix.Catalogo.UnitTests.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.CreateGenre;

[CollectionDefinition(nameof(CreateGenreTestFixtureCollection))]
public class CreateGenreTestFixtureCollection(): ICollectionFixture<GenreUseCasesBaseFixture>
{
    
}

public class CreateGenreTestFixture : GenreUseCasesBaseFixture
{
    
}