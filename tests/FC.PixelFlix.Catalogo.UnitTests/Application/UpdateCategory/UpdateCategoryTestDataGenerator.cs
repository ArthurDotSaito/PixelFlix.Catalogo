using FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.UpdateCategory;
public class UpdateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 0)
    {
        var fixture = new UpdateCategoryTestFixture();
        for(int i = 0; i < times; i++)
        {
            var aCategory = fixture.GetAValidCategory();
            var anRequest = fixture.GetValidRequest(aCategory.Id);

            yield return new object[] { aCategory, anRequest };
        }
    }
}
