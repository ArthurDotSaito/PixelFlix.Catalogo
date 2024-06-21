using System.Net;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FluentAssertions;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.CreateCategory;

[Collection(nameof(CreateCategoryApiTestFixtureCollection))]
public class CreateCategoryApiTest
{
    private readonly CreateCategoryApiTestFixture _fixture;
    public CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GivenAValidCreateCategoryRequest_WhenCallsCreateCategory_ShouldCreateCategory))]
    [Trait("E2E/API", "Category Endpoints")]
    public async Task GivenAValidCreateCategoryRequest_WhenCallsCreateCategory_ShouldCreateCategory()
    {
        //given
        var createCategoryRequest = _fixture.GetAValidCreateCategoryRequest();
        
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Post<CategoryModelResponse>("/categories", createCategoryRequest);
        
        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Should().NotBeNull();
        response!.Id.Should().NotBeEmpty();
        response.Name.Should().Be(createCategoryRequest.Name);
        response.Description.Should().Be(createCategoryRequest.Description);
        response.IsActive.Should().Be(createCategoryRequest.IsActive);
        response.CreatedAt.Should().NotBeSameDateAs(default);

        var categoryInDatabase = await _fixture.Persistence.GetById(response.Id);
        
        categoryInDatabase.Should().NotBeNull();
        categoryInDatabase!.Id.Should().Be(response.Id);
        categoryInDatabase.Name.Should().Be(response.Name);
        categoryInDatabase.Description.Should().Be(response.Description);
        categoryInDatabase.IsActive.Should().Be(response.IsActive);
        categoryInDatabase.CreatedAt.Should().NotBeSameDateAs(default);
    }
    
    [Fact(DisplayName = nameof(GivenAValidCreateCategoryRequest_WhenCallsCreateCategory_ShouldCreateCategory))]
    [Trait("E2E/API", "Category Endpoints")]
    public async Task GivenInvalidCreateCategoryRequest_WhenCallsCreateCategory_ShouldThrowAException()
    {
        //given
        var createCategoryRequest = _fixture.GetAValidCreateCategoryRequest();
        
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Post<CategoryModelResponse>("/categories", createCategoryRequest);
        
        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Should().NotBeNull();
        response!.Id.Should().NotBeEmpty();
        response.Name.Should().Be(createCategoryRequest.Name);
        response.Description.Should().Be(createCategoryRequest.Description);
        response.IsActive.Should().Be(createCategoryRequest.IsActive);
        response.CreatedAt.Should().NotBeSameDateAs(default);

        var categoryInDatabase = await _fixture.Persistence.GetById(response.Id);
        
        categoryInDatabase.Should().NotBeNull();
        categoryInDatabase!.Id.Should().Be(response.Id);
        categoryInDatabase.Name.Should().Be(response.Name);
        categoryInDatabase.Description.Should().Be(response.Description);
        categoryInDatabase.IsActive.Should().Be(response.IsActive);
        categoryInDatabase.CreatedAt.Should().NotBeSameDateAs(default);
    }
    
}