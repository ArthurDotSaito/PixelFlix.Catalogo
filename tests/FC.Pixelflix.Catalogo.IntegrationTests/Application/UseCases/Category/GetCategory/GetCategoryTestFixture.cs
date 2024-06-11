using FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestFixtureCollection))]
public class GetCategoryTestFixtureCollection: ICollectionFixture<GetCategoryTestFixture> { }

public class GetCategoryTestFixture: CategoryUseCaseBaseFixture
{
    
}