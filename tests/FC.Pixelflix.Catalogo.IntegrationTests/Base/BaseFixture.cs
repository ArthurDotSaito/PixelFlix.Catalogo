using Bogus;
using FC.Pixelflix.Catalogo.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Base;
public class BaseFixture
{
    protected Faker Faker {  get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
    }
    
    public PixelflixCatalogDbContext CreateDbContext(bool preserveData = false)
    {
        var dbContext = new PixelflixCatalogDbContext(
            new DbContextOptionsBuilder<PixelflixCatalogDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
        );

        if (preserveData == false) dbContext.Database.EnsureDeleted();

        return dbContext;
    }
}
