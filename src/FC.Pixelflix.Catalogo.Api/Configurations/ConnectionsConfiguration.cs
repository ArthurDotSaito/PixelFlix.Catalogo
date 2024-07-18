using FC.Pixelflix.Catalogo.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace FC.Pixelflix.Catalogo.Api.Configurations;

public static class ConnectionsConfiguration
{
    public static IServiceCollection AddAppConnections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbConnections(configuration);
        return services;
    }
    
    private static IServiceCollection AddDbConnections(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CatalogDb");
        services.AddDbContext<PixelflixCatalogDbContext>(
            options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        return services;
    }
}