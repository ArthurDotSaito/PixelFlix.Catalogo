using FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
using FC.Pixelflix.Catalogo.Domain.Exceptions;
using FC.Pixelflix.Catalogo.Infra.Data.EF;
using FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using useCases = FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.CreateCategory;

[Collection(nameof(CreateCategoryTestFixtureCollection))]
public class CreateCategoryTestIT
{
    private readonly CreateCategoryTestFixture _fixture;
    public CreateCategoryTestIT(CreateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsCreateCategory_shouldBeOk))]
    [Trait("Integration/Application ", "CreateCategory - Use Cases")]
    public async void GivenAValidCommand_whenCallsCreateCategory_shouldBeOk()
    {
        //given
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new useCases.CreateCategory(unitOfWork, repository);

        var request = _fixture.GetValidRequest();

        //when
        var response = await useCase.Execute(request, CancellationToken.None);

        PixelflixCatalogDbContext aSecondContext = _fixture.CreateDbContext(true);
        var dbCategory = await aSecondContext.Categories.FindAsync(response.Id);
        
        //then
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(request.Name);
        dbCategory.Description.Should().Be(request.Description);
        dbCategory.IsActive.Should().Be(request.IsActive);
        dbCategory.CreatedAt.Should().Be(response.CreatedAt);
        
        response.Should().NotBeNull();
        response.Id.Should().NotBeEmpty();
        response.Id.Should().NotBe(default(Guid));
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be(request.IsActive);
        response.CreatedAt.Should().NotBe(null);
        response.CreatedAt.Should().NotBeSameDateAs(default);
    }
    
    [Fact(DisplayName = nameof(GivenAValidCommandWithNameOnly_whenCallsCreateCategory_shouldPersistACategory))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void GivenAValidCommandWithNameOnly_whenCallsCreateCategory_shouldPersistACategory()
    {
        //given
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new useCases.CreateCategory(unitOfWork, repository);

        var request = new CreateCategoryRequest(_fixture.GetValidRequest().Name);
        //when
        var response = await useCase.Execute(request, CancellationToken.None);

        //then
        response.Should().NotBeNull();
        response.Id.Should().NotBeEmpty();
        response.Id.Should().NotBe(default(Guid));
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be("");
        response.IsActive.Should().Be(true);
        response.CreatedAt.Should().NotBe(null);
        response.CreatedAt.Should().NotBeSameDateAs(default);
    }
    
    [Fact(DisplayName = nameof(GivenAInvalidCommandWitNameAndDescription_whenCallsCreateCategory_shouldPersistCategory))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void GivenAInvalidCommandWitNameAndDescription_whenCallsCreateCategory_shouldPersistCategory()
    {
        //given
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new useCases.CreateCategory(unitOfWork, repository);

        var aCategoryRequest = _fixture.GetValidRequest();
        var request = new CreateCategoryRequest(aCategoryRequest.Name,aCategoryRequest.Description);
        
        //when
        var response = await useCase.Execute(request, CancellationToken.None);

        //then
        response.Should().NotBeNull();
        response.Id.Should().NotBeEmpty();
        response.Id.Should().NotBe(default(Guid));
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be(request.IsActive);
        response.CreatedAt.Should().NotBe(null);
        response.CreatedAt.Should().NotBeSameDateAs(default);
    }
    
    [Theory(DisplayName = nameof(GivenAInvalidCommand_whenCallsCreateCategory_shouldThrowsAnException))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    [MemberData(
        nameof(CreateCategoryTestDataGenerator.GetInvalidInput),
        parameters: 4,
        MemberType = typeof(CreateCategoryTestDataGenerator)
    )]
    public async void GivenAInvalidCommand_whenCallsCreateCategory_shouldThrowsAnException(
        CreateCategoryRequest request,
        string expectedExceptionMessage)
    {
        //given
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new useCases.CreateCategory(unitOfWork, repository);
        
        //when
        var task = async () => await useCase.Execute(request, CancellationToken.None);

        //then
        await task.Should().ThrowAsync<EntityValidationException>().WithMessage(expectedExceptionMessage);

        var aSecondContext = _fixture.CreateDbContext(true).Categories.AsNoTracking().ToList();

        aSecondContext.Should().HaveCount(0);
    }
}