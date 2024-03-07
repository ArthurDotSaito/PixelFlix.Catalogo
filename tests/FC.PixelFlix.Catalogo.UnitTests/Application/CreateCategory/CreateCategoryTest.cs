using FC.Pixelflix.Catalogo.Application.UseCases.Category.Dto;
using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.Exceptions;
using FC.PixelFlix.Catalogo.UnitTests.Application.CreateCategory ;
using FluentAssertions;
using Moq;
using Xunit;
using useCases = FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsCreateCategory_shouldBeOk))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void GivenAValidCommand_whenCallsCreateCategory_shouldBeOk()
    {
        var repositoryMock = _fixture.GetMockRepository();
        var unitOfWorkMock = _fixture.GetMockUnitOfWork();

        var useCase = new useCases.CreateCategory(unitOfWorkMock.Object, repositoryMock.Object);

        var input = _fixture.GetValidInput();

        var output = await useCase.Execute(input, CancellationToken.None);

        repositoryMock.Verify(repository => 
            repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), 
            Times.Once
            );

        unitOfWorkMock.Verify(uow => 
            uow.Commit(It.IsAny<CancellationToken>()),Times.Once
            );

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Id.Should().NotBe(default(Guid));
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBe(null);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Theory(DisplayName = nameof(GivenAInvalidCommand_whenCallsCreateCategory_shouldThrowsAnException))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(CreateCategoryDataGenerator.GetInvalidInput), MemberType = typeof(CreateCategoryDataGenerator))]

    public async void GivenAInvalidCommand_whenCallsCreateCategory_shouldThrowsAnException(
        CreateCategoryInput input, string expectedExceptionMessage)
    {
        var repositoryMock = _fixture.GetMockRepository();
        var unitOfWorkMock = _fixture.GetMockUnitOfWork();

        var useCase = new useCases.CreateCategory(unitOfWorkMock.Object, repositoryMock.Object);

        Func<Task> task = async () => await useCase.Execute(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(expectedExceptionMessage);
    }   

    [Fact(DisplayName = nameof(GivenAInvalidCommandWitName_whenCallsCreateCategory_shouldBeOk))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void GivenAInvalidCommandWitName_whenCallsCreateCategory_shouldBeOk()
    {
        var repositoryMock = _fixture.GetMockRepository();
        var unitOfWorkMock = _fixture.GetMockUnitOfWork();

        var useCase = new useCases.CreateCategory(unitOfWorkMock.Object, repositoryMock.Object);

        var input = new CreateCategoryInput(_fixture.GetValidCategoryName());

        var output = await useCase.Execute(input, CancellationToken.None);

        repositoryMock.Verify(repository =>
            repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once
            );

        unitOfWorkMock.Verify(uow =>
            uow.Commit(It.IsAny<CancellationToken>()), Times.Once
            );

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Id.Should().NotBe(default(Guid));
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be("");
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBe(null);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Fact(DisplayName = nameof(GivenAInvalidCommandWitNameAndDescription_whenCallsCreateCategory_shouldBeOk))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void GivenAInvalidCommandWitNameAndDescription_whenCallsCreateCategory_shouldBeOk()
    {
        var repositoryMock = _fixture.GetMockRepository();
        var unitOfWorkMock = _fixture.GetMockUnitOfWork();

        var useCase = new useCases.CreateCategory(unitOfWorkMock.Object, repositoryMock.Object);

        var input = new CreateCategoryInput(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

        var output = await useCase.Execute(input, CancellationToken.None);

        repositoryMock.Verify(repository =>
            repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once
            );

        unitOfWorkMock.Verify(uow =>
            uow.Commit(It.IsAny<CancellationToken>()), Times.Once
            );

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Id.Should().NotBe(default(Guid));
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBe(null);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

}
