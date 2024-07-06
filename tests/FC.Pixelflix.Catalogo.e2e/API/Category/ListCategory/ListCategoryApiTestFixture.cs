using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using FC.Pixelflix.Catalogo.e2e.API.Common;
using CategoryDomain = FC.Pixelflix.Catalogo.Domain.Entities.Category;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.ListCategory;

[CollectionDefinition(nameof(ListCategoryApiTestFixtureCollection))]
public class ListCategoryApiTestFixtureCollection : ICollectionFixture<ListCategoryApiTestFixture> { }

public class ListCategoryApiTestFixture : CategoryBaseFixture
{
    public List<CategoryDomain> GetValidCategoryListWithNames(List<string> names)
    {
        return names.Select(name =>
        {
            var category = GetValidCategory();
            category.Update(name);
            return category;
        }).ToList();
    }
    
    public List<CategoryDomain> CloneCategoryListListAndOrderIt(List<CategoryDomain> categories,string orderBy, SearchOrder searchOrder)
    {
        var newCategoriesList = new List<CategoryDomain>(categories);
        var newCategoriesListEnumerable = (orderBy.ToLower(), searchOrder) switch
        {

            ("name", SearchOrder.Asc) => newCategoriesList.OrderBy(items => items.Name),
            ("name", SearchOrder.Desc) => newCategoriesList.OrderByDescending(items => items.Name),
            ("id", SearchOrder.Asc) => newCategoriesList.OrderBy(items => items.Id),
            ("id", SearchOrder.Desc) => newCategoriesList.OrderByDescending(items => items.Id),
            ("createdat", SearchOrder.Asc) => newCategoriesList.OrderBy(items => items.CreatedAt),
            ("createdat", SearchOrder.Desc) => newCategoriesList.OrderByDescending(items => items.CreatedAt),
            _ => newCategoriesList.OrderBy(items => items.Name),
        };
 
        return newCategoriesListEnumerable.ThenBy(e=>e.CreatedAt).ToList();
    }
    
}