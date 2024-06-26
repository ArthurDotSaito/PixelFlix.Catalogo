using FC.Pixelflix.Catalogo.e2e.API.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryApiTestCollection))]
public class DeleteCategoryApiTestCollection: ICollectionFixture<DeleteCategoryApiTestFixture>{}

public class DeleteCategoryApiTestFixture : CategoryBaseFixture
{
    
}