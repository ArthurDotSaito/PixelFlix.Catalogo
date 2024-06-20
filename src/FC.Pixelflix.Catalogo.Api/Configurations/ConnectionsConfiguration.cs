using FC.Pixelflix.Catalogo.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace FC.Pixelflix.Catalogo.Api.Configurations;

public static class ConnectionsConfiguration
{
    public static IServiceCollection AddAppConnections(this IServiceCollection services)
    {
        services.AddDbConnections();
        return services;
    }
    
    private static IServiceCollection AddDbConnections(this IServiceCollection services)
    {
        services.AddDbContext<PixelflixCatalogDbContext>(
            options => options.UseInMemoryDatabase("InMemory-DSV-database"));
        return services;
    }
}