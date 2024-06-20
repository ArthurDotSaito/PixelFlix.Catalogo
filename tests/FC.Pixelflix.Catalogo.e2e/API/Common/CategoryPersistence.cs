using FC.Pixelflix.Catalogo.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace FC.Pixelflix.Catalogo.e2e.API.Common;
using CategoryDomain = FC.Pixelflix.Catalogo.Domain.Entities.Category;

public class CategoryPersistence
{
    private readonly PixelflixCatalogDbContext _context;
    
    public CategoryPersistence(PixelflixCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDomain?> GetById(Guid anId)
    {
        return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c=>c.Id == anId);
    } 
    
}