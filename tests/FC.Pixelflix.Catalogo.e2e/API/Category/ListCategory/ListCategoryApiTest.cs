using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.ListCategory;

[Collection(nameof(ListCategoryApiTestFixtureCollection))]
public class ListCategoryApiTest
{
    private readonly ListCategoryApiTestFixture _fixture;
    
    public ListCategoryApiTest(ListCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }
    
}