using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryApiTestCollection))]
public class DeleteCategoryApiTest
{
    private readonly DeleteCategoryApiTestFixture _fixture;
    
    public DeleteCategoryApiTest(DeleteCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    
}