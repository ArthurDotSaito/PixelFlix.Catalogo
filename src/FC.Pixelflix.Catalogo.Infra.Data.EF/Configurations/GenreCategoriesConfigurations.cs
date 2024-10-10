using FC.Pixelflix.Catalogo.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FC.Pixelflix.Catalogo.Infra.Data.EF.Configurations;

public class GenreCategoriesConfigurations : IEntityTypeConfiguration<GenresCategories>
{
    public void Configure(EntityTypeBuilder<GenresCategories> builder)
    {
        builder.HasKey(relation => new {relation.CategoryId, relation.GenreId});
    }
}