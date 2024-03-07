namespace FC.PixelFlix.Catalogo.UnitTests.Application.CreateCategory;
public class CreateCategoryDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInput()
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputList = new List<object[]>();

        var invalidInputShortName = fixture.GetValidInput();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
        invalidInputList.Add(new object[]
        {
            invalidInputShortName,
            "Name should be at least 3 characters long"
        });

        var invalidInputLongName = fixture.GetValidInput();
        var longName = fixture.Faker.Commerce.ProductName(); ;
        while (longName.Length < 255)
        {
            longName = $"{longName}{fixture.Faker.Commerce.ProductName()}";
        }
        invalidInputLongName.Name = longName;
        invalidInputList.Add(new object[]
        {
            invalidInputLongName,
            "Name should be less than 255 characters long"
        });

        var invalidInputDescriptionNull = fixture.GetValidInput();
        invalidInputDescriptionNull.Description = null!;
        invalidInputList.Add(new object[]
        {
            invalidInputDescriptionNull,
            "Description should not be null"
        });

        var invalidInputLongDescription = fixture.GetValidInput();
        var longDescription = fixture.Faker.Commerce.ProductDescription(); ;
        while (longDescription.Length < 10000)
        {
            longDescription = $"{longDescription}{fixture.Faker.Commerce.ProductDescription()}";
        }
        invalidInputLongDescription.Description = longDescription;
        invalidInputList.Add(new object[]
        {
            invalidInputLongDescription,
            "Description should be less than 10000 characters long"
        });


        return invalidInputList;
    }
}
