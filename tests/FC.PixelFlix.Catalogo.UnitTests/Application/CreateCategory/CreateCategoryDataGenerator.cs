namespace FC.PixelFlix.Catalogo.UnitTests.Application.CreateCategory;
public class CreateCategoryDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInput(int times = 12)
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputList = new List<object[]>();
        var totalInvalidCases = 4;

        for (int i = 0;i < times; i++)
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

            }
        }

        invalidInputList.Add(new object[]
        {
            fixture.GetInvalidShortNameInput(),
            "Name should be at least 3 characters long"
        });

        invalidInputList.Add(new object[]
        {
            fixture.GetInvalidLongNameInput(),
            "Name should be less than 255 characters long"
        });

        invalidInputList.Add(new object[]
        {
            fixture.GetNullDescription(),
            "Description should not be null"
        });

        invalidInputList.Add(new object[]
        {
            fixture.GetInvalidLongDescription(),
            "Description should be less than 10000 characters long"
        });


        return invalidInputList;
    }
}
