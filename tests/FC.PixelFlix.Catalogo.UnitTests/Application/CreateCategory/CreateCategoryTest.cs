using FC.Pixelflix.Catalogo.Application.UseCases.Category.Dto;
using FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.Exceptions;
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

    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsCreateCategory_shouldReturnACategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void GivenAValidCommand_whenCallsCreateCategory_shouldReturnACategory()
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
    [MemberData(nameof(GetInvalidInput))]
    public async void GivenAInvalidCommand_whenCallsCreateCategory_shouldThrowsAnException(
        CreateCategoryInput input, string expectedExceptionMessage)
    {
        var repositoryMock = _fixture.GetMockRepository();
        var unitOfWorkMock = _fixture.GetMockUnitOfWork();

        var useCase = new useCases.CreateCategory(unitOfWorkMock.Object, repositoryMock.Object);

        Func<Task> task = async () => await useCase.Execute(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(expectedExceptionMessage);
    }   

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
        while(longName.Length < 255)
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



        return invalidInputList;
    }
}
