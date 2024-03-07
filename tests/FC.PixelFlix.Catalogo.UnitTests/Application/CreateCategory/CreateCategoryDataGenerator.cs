namespace FC.PixelFlix.Catalogo.UnitTests.Application.CreateCategory;
public class CreateCategoryDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInput()
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputList = new List<object[]>();

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
