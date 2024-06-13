using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixtureCollection))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;
    
    public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }
}