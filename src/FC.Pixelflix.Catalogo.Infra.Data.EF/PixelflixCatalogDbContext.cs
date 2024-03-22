using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FC.Pixelflix.Catalogo.Infra.Data.EF;
public class PixelflixCatalogDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();

    public PixelflixCatalogDbContext(DbContextOptions<PixelflixCatalogDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CategoryConfiguration());    
    }

}
