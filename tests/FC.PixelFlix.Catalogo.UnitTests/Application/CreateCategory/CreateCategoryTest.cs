using FC.Pixelflix.Catalogo.Domain.Entities;
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
        var expectedName = "categoryName";
        var expectedDescription = "A category description";
        var expectedIsActive = true;

        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new useCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var input = new CreateCategoryInput(expectedName, expectedDescription, expectedIsActive);

        var output = await useCase.Execute(input, CancellationToken.None);

        repositoryMock.Verify(repository => repository.Create(It.IsAny<Category>(), CancellationToken.None), Times.Once);
        unitOfWorkMock.Verify(uow => uow.Commit(),Times.Once);

        output.Should().NotBeNull();
        output.Id.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Id.Should().NotBe(default(Guid));
        output.Name.Should().BeEqual(expectedName);
        output.Description.Should().BeEqual(expectedDescription);
        output.IsActive.Should().BeTrue();
        output.CreatedAt.Should().NotBeNull();
        output.CreatedAt.Should().NotBe(default(DateTime));
    }
}
