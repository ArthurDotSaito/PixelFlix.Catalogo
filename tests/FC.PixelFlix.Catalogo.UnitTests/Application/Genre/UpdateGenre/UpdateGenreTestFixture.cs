using FC.PixelFlix.Catalogo.UnitTests.Application.Genre.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.UpdateGenre;

[CollectionDefinition(nameof(UpdateGenreTestFixture))]
public class CreateGenreTestFixtureCollection(): ICollectionFixture<UpdateGenreTestFixture> { }

public class UpdateGenreTestFixture : GenreUseCasesBaseFixture
{
    
}