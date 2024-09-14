using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Domain.Repository;
using FC.PixelFlix.Catalogo.UnitTests.Common;
using Moq;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.Common;

public class GenreUseCasesBaseFixture : BaseFixture
{
    public string GetValidGenreName()
    {
        return Faker.Commerce.Categories(1)[0];
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