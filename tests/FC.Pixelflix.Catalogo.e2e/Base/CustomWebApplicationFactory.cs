using FC.Pixelflix.Catalogo.Infra.Data.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Pixelflix.Catalogo.e2e.Base;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("e2e-test");
        builder.ConfigureServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<PixelflixCatalogDbContext>();
                if (dbContext is null) throw new ArgumentNullException(nameof(dbContext));
                
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
            
            // ---- InMemoryDatabase ---- //
            
            /*var databaseOptions = services.FirstOrDefault(s => s.ServiceType == typeof(DbContextOptions<PixelflixCatalogDbContext>));
            if (databaseOptions is not null)
            {
                services.Remove(databaseOptions);
            }

            services.AddDbContext<PixelflixCatalogDbContext>(options =>
            {
                options.UseInMemoryDatabase("e2e-tests-db");
            });*/
        });
        
        base.ConfigureWebHost(builder);
    }
}