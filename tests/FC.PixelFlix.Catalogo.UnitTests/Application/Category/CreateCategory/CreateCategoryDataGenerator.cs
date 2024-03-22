using FC.PixelFlix.Catalogo.UnitTests.Application.Category.CreateCategory;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Category.CreateCategory;
public class CreateCategoryDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInput(int times = 12)
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputList = new List<object[]>();
        var totalInvalidCases = 4;

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
                        fixture.GetNullDescription(),
                        "Description should not be null"
                    });
                    break;
                case 3:
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
