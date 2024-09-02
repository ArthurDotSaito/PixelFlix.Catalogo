using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.PixelFlix.Catalogo.UnitTests.Application.Genre.Common;
using Moq;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.CreateGenre;

[CollectionDefinition(nameof(CreateGenreTestFixtureCollection))]
public class CreateGenreTestFixtureCollection(): ICollectionFixture<GenreUseCasesBaseFixture> { }

public class CreateGenreTestFixture : GenreUseCasesBaseFixture
{
    public CreateGenreRequest GetValidInput()
    {
        var genreName = GetValidGenreName();
        var isActive = GetRandomIsActive();
        
        return new CreateGenreRequest(genreName, isActive);
    }
    
    public Mock<IGenreRepository> GetGenreRepositoryMock()
    {
        return new Mock<GenreRepository>();
    }
    
    public Mock<IUnitOfWork> GetUnitOfWorkMock()
    {
        return new Mock<IUnitOfWork>();
    }
}