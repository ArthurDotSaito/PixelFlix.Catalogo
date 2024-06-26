﻿namespace FC.Pixelflix.Catalogo.e2e.API.Category.CreateCategory;

public class CreateCategoryApiTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInput()
    {
        var fixture = new CreateCategoryApiTestFixture();
        var invalidInputList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int i = 0; i < totalInvalidCases; i++)
        {
            switch (i % totalInvalidCases)
            {
                case 0:
                    var requestShortName = fixture.GetAValidCreateCategoryRequest();
                    requestShortName.Name = fixture.GetInvalidShortName();
                    invalidInputList.Add(new object[]
                    {
                        
                        requestShortName,
                        "Name should be at least 3 characters long"
                    });
                    break;

                case 1:
                    var requestLongName = fixture.GetAValidCreateCategoryRequest();
                    requestLongName.Name = fixture.GetInvalidLongName();
                    invalidInputList.Add(new object[]
                    {
                        requestLongName,
                        "Name should be less than 255 characters long"
                    });
                    break;
                case 2:
                    var requestLongDescription = fixture.GetAValidCreateCategoryRequest();
                    requestLongDescription.Description = fixture.GetInvalidLongDescription();
                    invalidInputList.Add(new object[]
                    {
                        requestLongDescription,
                        "Description should be less than 10000 characters long"
                    });
                    break;
            }
        }

        return invalidInputList;
    }
}