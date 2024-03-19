using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.SeedWork;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;

namespace FC.Pixelflix.Catalogo.Domain.Repository;
public interface ICategoryRepository : IGenericRepository<Category>, ISearchableRepository<Category>
{
   
}
