using Moq;
using Xunit;
using useCases = FC.PixelFlix.Catalogo.Application.UseCases.CreateCategory;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.CreateCategory;
public class CreateCategoryTest
{
    [Fact(DisplayName = nameof()]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var useCase = new useCases.CreateCategory(repositoryMock.Object, unitOfWork.Object);

        var input = new CreateCategoryInput("categoryName", "categoryDescription", true);

        var output = await useCase.Execute(input);
    }
}
