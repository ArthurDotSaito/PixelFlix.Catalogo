using FC.PixelFlix.Catalogo.UnitTests.Application.Category.UpdateCategory;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Category.UpdateCategory;
public class UpdateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 0)
    {
        var fixture = new UpdateCategoryTestFixture();
        for (int i = 0; i < times; i++)
        {
            var aCategory = fixture.GetValidCategory();
            var anRequest = fixture.GetValidRequest(aCategory.Id);

            yield return new object[] { aCategory, anRequest };
        }
    }

    public static IEnumerable<object[]> GetInvalidInput(int times = 12)
    {
        var fixture = new UpdateCategoryTestFixture();
        var invalidInputList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int i = 0; i < times; i++)
        {
            switch (i % totalInvalidCases)
            {
                case 0:
                    invalidInputList.Add(new object[]
                    {
                        fixture.GetInvalidShortNameInput(),
                        "Name should be at least 3 characters long"
                    });
                    break;

                case 1:
                    invalidInputList.Add(new object[]
                    {
                        fixture.GetInvalidLongNameInput(),
                        "Name should be less than 255 characters long"
                    });
                    break;
                case 2:
                    invalidInputList.Add(new object[]
                    {
                        fixture.GetInvalidLongDescription(),
                        "Description should be less than 10000 characters long"
                   });
                    break;
                default:
                    break;

            }
        }

        return invalidInputList;
    }
}
