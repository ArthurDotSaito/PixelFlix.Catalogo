using Bogus;
using FC.Pixelflix.Catalogo.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace FC.Pixelflix.Catalogo.e2e.Base;

public class BaseFixture
{
    protected Faker Faker {  get; set; }
    public ApiClient ApiClient { get; set; }
    
    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
    }
    
    public PixelflixCatalogDbContext CreateDbContext(bool preserveData = false)
    {
        var dbContext = new PixelflixCatalogDbContext(
            new DbContextOptionsBuilder<PixelflixCatalogDbContext>()
                .UseInMemoryDatabase("e2e-tests-db")
                .Options
        );
        return dbContext;
    }
}