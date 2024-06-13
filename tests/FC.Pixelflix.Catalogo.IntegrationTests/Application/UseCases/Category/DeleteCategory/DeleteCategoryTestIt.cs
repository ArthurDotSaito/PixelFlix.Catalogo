using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixtureCollection))]
public class DeleteCategoryTestIt
{
    private readonly DeleteCategoryTestFixture _fixture;
    
    public DeleteCategoryTestIt(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    
}