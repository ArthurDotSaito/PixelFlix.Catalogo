using FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.ListCategories;

[CollectionDefinition(nameof(ListCategoriesTestFixtureColllection))]
public class ListCategoriesTestFixtureColllection : ICollectionFixture<ListCategoriesTestFixture>{ }

public class ListCategoriesTestFixture : CategoryUseCaseBaseFixture
{
    
}