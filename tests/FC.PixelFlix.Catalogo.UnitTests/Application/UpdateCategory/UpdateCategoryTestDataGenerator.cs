using FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.UpdateCategory;
public class UpdateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 0)
    {
        var fixture = new UpdateCategoryTestFixture();
        for(int i = 0; i < times; i++)
        {
            var aName = fixture.GetValidCategoryName();
            var aDescription = fixture.GetValidCategoryDescription();
            var anIsActive = fixture.GetRandomIsActive();

            var aCategory = fixture.GetAValidCategory();
            var anRequest = new UpdateCategoryRequest(aCategory.Id, aName, aDescription, anIsActive);

            yield return new object[] { aCategory, anRequest };
        }
    }
}
