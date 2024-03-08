using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryFixtureCollection))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }


}
