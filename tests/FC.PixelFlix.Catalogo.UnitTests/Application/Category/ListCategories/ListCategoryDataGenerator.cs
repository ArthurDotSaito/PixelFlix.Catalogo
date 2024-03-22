using FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Category.ListCategories;
public class ListCategoryDataGenerator
{
    public static IEnumerable<object[]> GetRequestWithDefaultParams(int times = 12)
    {
        var fixture = new ListCategoriesTestFixture();

        var aRequest = fixture.GetValidRequest();

        for (int i = 0; i < times; i++)
        {
            switch (i % 6)
            {
                case 0:
                    yield return new object[] { new ListCategoriesRequest() };
                    break;
                case 1:
                    yield return new object[] { new ListCategoriesRequest(aRequest.Page) };
                    break;
                case 2:
                    yield return new object[] { new ListCategoriesRequest(aRequest.Page, aRequest.PerPage) };
                    break;
                case 3:
                    yield return new object[] { new ListCategoriesRequest(aRequest.Page, aRequest.PerPage, aRequest.Search) };
                    break;
                case 4:
                    yield return new object[] { new ListCategoriesRequest(aRequest.Page, aRequest.PerPage, aRequest.Search, aRequest.Sort) };
                    break;
                case 5:
                    yield return new object[] { aRequest };
                    break;
                default:
                    yield return new object[] { new ListCategoriesRequest() };
                    break;
            }
        }
    }
}
