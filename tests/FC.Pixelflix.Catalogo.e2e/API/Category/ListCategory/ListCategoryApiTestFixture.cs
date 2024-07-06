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
    
}