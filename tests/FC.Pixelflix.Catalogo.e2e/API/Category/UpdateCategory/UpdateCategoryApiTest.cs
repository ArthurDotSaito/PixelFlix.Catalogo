using System.Net;
using FC.Pixelflix.Catalogo.Api.ApiModels.Category;
using FC.Pixelflix.Catalogo.Api.ApiModels.Response;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryFixtureCollection))]
public class UpdateCategoryApiTest : IDisposable
{
    private readonly UpdateCategoryApiTestFixture _fixture;
    
    public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(GivenAValidId_whenCallsUpdateCategory_shouldReturnACategory))]
    [Trait("E2E/Api", "UpdateCategory - Endpoints")]
    public async void GivenAValidId_whenCallsUpdateCategory_shouldReturnACategory()
    {
        //given
        var categoriesList = _fixture.GetValidCategoryList(20);
        await _fixture.Persistence.InsertList(categoriesList);
        
        var aCategory = categoriesList[10];
        var request = _fixture.GetAValidUpdateCategoryApiRequest();
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Put<ApiResponse<CategoryModelResponse>>($"/categories/{aCategory.Id}", request);
        
        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response!.Data.Id.Should().Be(aCategory.Id);
        response.Data.Name.Should().Be(request.Name);
        response.Data.Description.Should().Be(request.Description);
        response.Data.IsActive.Should().Be((bool)request.IsActive!);

        var categoryInDatabase = await _fixture.Persistence.GetById(aCategory.Id);
        
        categoryInDatabase.Should().NotBeNull();
        categoryInDatabase!.Id.Should().Be(aCategory.Id);
        categoryInDatabase.Name.Should().Be(request.Name);
        categoryInDatabase.Description.Should().Be(request.Description);
        categoryInDatabase.IsActive.Should().Be((bool)request.IsActive);
    }
    
    [Fact(DisplayName = nameof(GivenAValidId_whenCallsUpdateCategoryWithNameOnly_shouldReturnACategory))]
    [Trait("E2E/Api", "UpdateCategory - Endpoints")]
    public async void GivenAValidId_whenCallsUpdateCategoryWithNameOnly_shouldReturnACategory()
    {
        //given
        var categoriesList = _fixture.GetValidCategoryList(20);
        await _fixture.Persistence.InsertList(categoriesList);
        
        var aCategory = categoriesList[10];
        var request = new UpdateCategoryRequest(aCategory.Id, _fixture.GetValidCategoryName())
        {
            IsActive = aCategory.IsActive
        };
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Put<ApiResponse<CategoryModelResponse>>($"/categories/{aCategory.Id}", request);
        
        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response!.Data.Id.Should().Be(aCategory.Id);
        response.Data.Name.Should().Be(request.Name);
        response.Data.Description.Should().Be(aCategory.Description);
        response.Data.IsActive.Should().Be(aCategory.IsActive);

        var categoryInDatabase = await _fixture.Persistence.GetById(aCategory.Id);
        
        categoryInDatabase.Should().NotBeNull();
        categoryInDatabase!.Id.Should().Be(request.Id);
        categoryInDatabase.Name.Should().Be(request.Name);
        categoryInDatabase.Description.Should().Be(aCategory.Description);
        categoryInDatabase.IsActive.Should().Be(aCategory.IsActive);
    }
    
    [Fact(DisplayName = nameof(GivenAValidId_whenCallsUpdateCategoryWithNameAndDescription_shouldReturnACategory))]
    [Trait("E2E/Api", "UpdateCategory - Endpoints")]
    public async void GivenAValidId_whenCallsUpdateCategoryWithNameAndDescription_shouldReturnACategory()
    {
        //given
        var categoriesList = _fixture.GetValidCategoryList(20);
        await _fixture.Persistence.InsertList(categoriesList);
        
        var aCategory = categoriesList[10];
        var request = new UpdateCategoryRequest(aCategory.Id, _fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription())
        {
            IsActive = aCategory.IsActive
        };
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Put<ApiResponse<CategoryModelResponse>>($"/categories/{aCategory.Id}", request);
        
        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response!.Data.Id.Should().Be(aCategory.Id);
        response.Data.Name.Should().Be(request.Name);
        response.Data.Description.Should().Be(request.Description);
        response.Data.IsActive.Should().Be(aCategory.IsActive);

        var categoryInDatabase = await _fixture.Persistence.GetById(aCategory.Id);
        
        categoryInDatabase.Should().NotBeNull();
        categoryInDatabase!.Id.Should().Be(request.Id);
        categoryInDatabase.Name.Should().Be(request.Name);
        categoryInDatabase.Description.Should().Be(request.Description);
        categoryInDatabase.IsActive.Should().Be(aCategory.IsActive);
    }
    
    [Fact(DisplayName = nameof(GivenAInvalidId_whenCallsUpdateCategory_shouldThrowsANotFound))]
    [Trait("E2E/Api", "UpdateCategory - Endpoints")]
    public async void GivenAInvalidId_whenCallsUpdateCategory_shouldThrowsANotFound()
    {
        //given
        var categoriesList = _fixture.GetValidCategoryList(20);
        await _fixture.Persistence.InsertList(categoriesList);

        var anId = Guid.NewGuid();
        var request = _fixture.GetAValidUpdateCategoryApiRequest();
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Put<ProblemDetails>($"/categories/{anId}", request);
        
        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Should().NotBeNull();
        response!.Title.Should().Be("Not Found");
        response!.Type.Should().Be("NotFound");
        response!.Status.Should().Be((int)HttpStatusCode.NotFound);
        response!.Detail.Should().Be($"Category '{anId}' not found.");
    }
    
    [Theory(DisplayName = nameof(GivenAInvalidCategoryRequest_whenCallsUpdateCategory_shouldThrowsUnprocessableEntity))]
    [Trait("E2E/Api", "UpdateCategory - Endpoints")]
    [MemberData(nameof(UpdateCategoryApiTestDataGenerator.GetInvalidInput), MemberType = typeof(UpdateCategoryApiTestDataGenerator))] 
    public async void GivenAInvalidCategoryRequest_whenCallsUpdateCategory_shouldThrowsUnprocessableEntity(
        UpdateCategoryApiRequest request,
        string expectedErrorMessage)
    {
        //given
        var categoriesList = _fixture.GetValidCategoryList(20);
        await _fixture.Persistence.InsertList(categoriesList);
        var aCategory = categoriesList[10];
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Put<ProblemDetails>($"/categories/{aCategory.Id}", request);
        
        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        response.Should().NotBeNull();
        response!.Title.Should().Be("One or more validation errors occurred.");
        response!.Type.Should().Be("UnprocessableEntity");
        response!.Status.Should().Be((int)HttpStatusCode.UnprocessableEntity);
        response!.Detail.Should().Be(expectedErrorMessage);
    }

    public void Dispose() => _fixture.CleanDatabase();
}