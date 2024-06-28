using FC.Pixelflix.Catalogo.e2e.API.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.ListCategory;

[CollectionDefinition(nameof(ListCategoryApiTestFixtureCollection))]
public class ListCategoryApiTestFixtureCollection : ICollectionFixture<ListCategoryApiTestFixture> { }

public class ListCategoryApiTestFixture : CategoryBaseFixture { }