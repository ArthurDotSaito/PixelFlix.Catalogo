using FC.Pixelflix.Catalogo.e2e.API.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.GetCategoryById;

[CollectionDefinition(nameof(GetCategoryApiTestFixtureCollection))]
public class GetCategoryApiTestFixtureCollection : ICollectionFixture<GetCategoryApiTestFixture> { }

public class GetCategoryApiTestFixture : CategoryBaseFixture
{
}