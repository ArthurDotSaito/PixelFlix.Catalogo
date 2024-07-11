using Bogus;
using FC.Pixelflix.Catalogo.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace FC.Pixelflix.Catalogo.e2e.Base;

public class BaseFixture
{
    protected Faker Faker {  get; set; }
    public ApiClient ApiClient { get; set; }
    public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
    public HttpClient HttpClient { get; set; }

    private readonly string _connectionString;
    
    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        HttpClient = WebAppFactory.CreateClient();
        ApiClient = new ApiClient(HttpClient);
        var configuration = WebAppFactory.Services.GetService(typeof(IConfiguration));
        
        ArgumentNullException.ThrowIfNull(configuration);

        _connectionString = ((IConfiguration)configuration).GetConnectionString("CatalogDb");
    }
    
    public PixelflixCatalogDbContext CreateDbContext(bool preserveData = false)
    {
        /*var dbContext = new PixelflixCatalogDbContext(
            new DbContextOptionsBuilder<PixelflixCatalogDbContext>()
                .UseInMemoryDatabase("e2e-tests-db")
                .Options
        );*/
        var dbContext = new PixelflixCatalogDbContext(
            new DbContextOptionsBuilder<PixelflixCatalogDbContext>()
                .
                UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString))
                .Options
        );
        return dbContext;
    }
    
    public void CleanDatabase()
    {
        var dbContext = CreateDbContext();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }
}