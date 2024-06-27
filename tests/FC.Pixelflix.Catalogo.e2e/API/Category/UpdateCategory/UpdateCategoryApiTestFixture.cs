using FC.Pixelflix.Catalogo.e2e.API.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryFixtureCollection))]
public class UpdateCategoryFixtureCollection : ICollectionFixture<UpdateCategoryApiTestFixture>
{
}

public class UpdateCategoryApiTestFixture : CategoryBaseFixture
{
    
}