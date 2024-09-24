using FC.PixelFlix.Catalogo.UnitTests.Application.Genre.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.DeleteGenre;

[CollectionDefinition(nameof(DeleteGenreTestFixture))]
public class CreateGenreTestFixtureCollection(): ICollectionFixture<DeleteGenreTestFixture> { }

public class DeleteGenreTestFixture : GenreUseCasesBaseFixture
{
    
}