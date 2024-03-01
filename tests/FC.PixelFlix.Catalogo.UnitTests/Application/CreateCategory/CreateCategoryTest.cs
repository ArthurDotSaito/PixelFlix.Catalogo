using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.Repository;
using Moq;
using Xunit;
using useCaseData = FC.Pixelflix.Catalogo.Application.UseCases.Category.Dto;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.CreateCategory;
public class CreateCategoryTest
{
    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsCreateCategory_shouldReturnACategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void GivenAValidCommand_whenCallsCreateCategory_shouldReturnACategory()
    {
        var expectedName = "categoryName";
        var expectedDescription = "A category description";
        var expectedIsActive = true;

        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new useCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        var input = new useCaseData.CreateCategoryInput(expectedName, expectedDescription, expectedIsActive);

        var output = await useCase.Execute(input, CancellationToken.None);

        repositoryMock.Verify(repository => 
            repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), 
            Times.Once
            );

        unitOfWorkMock.Verify(uow => 
            uow.Commit(It.IsAny<CancellationToken>()),Times.Once
            );

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
