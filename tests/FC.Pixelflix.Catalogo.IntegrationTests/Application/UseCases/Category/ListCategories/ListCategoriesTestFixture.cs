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
}