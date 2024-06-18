using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.Common;
using Xunit;
using CategoryDomain = FC.Pixelflix.Catalogo.Domain.Entities.Category;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.ListCategories;

[CollectionDefinition(nameof(ListCategoriesTestFixtureColllection))]
public class ListCategoriesTestFixtureColllection : ICollectionFixture<ListCategoriesTestFixture>{ }

public class ListCategoriesTestFixture : CategoryUseCaseBaseFixture
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

        return newCategoriesListEnumerable.ToList();
    }
}