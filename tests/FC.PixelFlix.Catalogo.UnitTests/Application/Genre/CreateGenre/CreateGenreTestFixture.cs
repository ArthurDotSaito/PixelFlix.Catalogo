using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.CreateGenre.Dto;
using FC.Pixelflix.Catalogo.Domain.Repository;
using FC.PixelFlix.Catalogo.UnitTests.Application.Genre.Common;
using Moq;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.CreateGenre;

[CollectionDefinition(nameof(CreateGenreTestFixture))]
public class CreateGenreTestFixtureCollection(): ICollectionFixture<CreateGenreTestFixture> { }

public class CreateGenreTestFixture : GenreUseCasesBaseFixture
{
    public CreateGenreRequest GetValidInput()
    {
        var genreName = GetValidGenreName();
        var isActive = GetRandomIsActive();
        
        return new CreateGenreRequest(genreName, isActive);
    }
    
    public CreateGenreRequest GetValidInputWithCategories()
    {
        var genreName = GetValidGenreName();
        var isActive = GetRandomIsActive();
        var categoriesIds = Enumerable.Range(0, (new Random()).Next(1, 10)).Select(_ => Guid.NewGuid()).ToList();
        
        return new CreateGenreRequest(genreName, isActive, categoriesIds);
    }
    
    public Mock<IGenreRepository> GetGenreRepositoryMock()
    {
        return new Mock<IGenreRepository>();
    }
    
    public Mock<IUnitOfWork> GetUnitOfWorkMock()
    {
        return new Mock<IUnitOfWork>();
    }
    
    public Mock<ICategoryRepository> GetCategoryRepositoryMock()
    {
        return new Mock<ICategoryRepository>();
    }
}