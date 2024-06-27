using System.Net;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryFixtureCollection))]
public class UpdateCategoryApiTest
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
        var request = _fixture.GetAValidUpdateCategoryRequest(aCategory.Id);
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Put<CategoryModelResponse>($"/categories/{aCategory.Id}", request);
        
        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        response!.Id.Should().Be(aCategory.Id);
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be((bool)request.IsActive!);

        var categoryInDatabase = await _fixture.Persistence.GetById(aCategory.Id);
        
        categoryInDatabase.Should().NotBeNull();
        categoryInDatabase!.Id.Should().Be(request.Id);
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
        var (responseMessage, response) = await _fixture.ApiClient.Put<CategoryModelResponse>($"/categories/{aCategory.Id}", request);
        
        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        response!.Id.Should().Be(aCategory.Id);
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(aCategory.Description);
        response.IsActive.Should().Be(aCategory.IsActive);

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
        var (responseMessage, response) = await _fixture.ApiClient.Put<CategoryModelResponse>($"/categories/{aCategory.Id}", request);
        
        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        response!.Id.Should().Be(aCategory.Id);
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.IsActive.Should().Be(aCategory.IsActive);

        var categoryInDatabase = await _fixture.Persistence.GetById(aCategory.Id);
        
        categoryInDatabase.Should().NotBeNull();
        categoryInDatabase!.Id.Should().Be(request.Id);
        categoryInDatabase.Name.Should().Be(request.Name);
        categoryInDatabase.Description.Should().Be(request.Description);
        categoryInDatabase.IsActive.Should().Be(aCategory.IsActive);
    }
}