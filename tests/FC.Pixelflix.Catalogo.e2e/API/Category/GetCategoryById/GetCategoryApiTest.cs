using System.Net;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.e2e.Extensions.DateTime;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.GetCategoryById;

[Collection(nameof(GetCategoryApiTestFixtureCollection))]
public class GetCategoryApiTest : IDisposable
{
    private readonly GetCategoryApiTestFixture _fixture;
    
    public GetCategoryApiTest(GetCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(GivenAValidId_whenCallsGetCategory_shouldReturnACategory))]
    [Trait("E2E/Api", "GetCategory - Endpoints")]
    public async Task GivenAValidId_whenCallsGetCategory_shouldReturnACategory()
    {
        //given
        var categoriesList = _fixture.GetValidCategoryList(20);
        await _fixture.Persistence.InsertList(categoriesList);
        
        var aCategory = categoriesList[10];
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Get<CategoryModelResponse>($"/categories/{aCategory.Id}");

        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Id.Should().Be(aCategory.Id);
        response.Name.Should().Be(aCategory.Name);
        response.Description.Should().Be(aCategory.Description);
        response.IsActive.Should().Be(aCategory.IsActive);
        response.CreatedAt.TrimMilliseconds().Should().Be(aCategory.CreatedAt.TrimMilliseconds());
    }
    
    [Fact(DisplayName = nameof(GivenAValidId_whenCallsGetCategory_shouldReturnACategory))]
    [Trait("E2E/Api", "GetCategory - Endpoints")]
    public async Task GivenAInvalidId_whenCallsGetCategory_shouldReturnNotFoundException()
    {
        //given
        var categoriesList = _fixture.GetValidCategoryList(20);
        await _fixture.Persistence.InsertList(categoriesList);
        
        var anId = Guid.NewGuid();
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Get<ProblemDetails>($"/categories/{anId}");

        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status404NotFound);
        response.Should().NotBeNull();
        response!.Status.Should().Be(StatusCodes.Status404NotFound);
        response.Title.Should().Be("Not Found");
        response.Detail.Should().Be($"Category '{anId}' not found.");
        response.Type.Should().Be("NotFound");
    }

    public void Dispose() => _fixture.CleanDatabase();
}