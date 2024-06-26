using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryApiTestCollection))]
public class DeleteCategoryApiTest
{
    private readonly DeleteCategoryApiTestFixture _fixture;
    
    public DeleteCategoryApiTest(DeleteCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GivenAValidId_whenCallsDeleteById_shouldDeleteThisCategory))]
    [Trait("E2E/Api", "DeleteCategory - Endpoints")]
    public async Task GivenAValidId_whenCallsDeleteById_shouldDeleteThisCategory()
    {
        //given
        var categoriesList = _fixture.GetValidCategoryList(20);
        await _fixture.Persistence.InsertList(categoriesList);
        
        var aCategory = categoriesList[10];
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Delete<object>($"/categories/{aCategory.Id}");

        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status204NoContent);
        response.Should().BeNull();
        
        var deletedCategory = await _fixture.Persistence.GetById(aCategory.Id);
        
        deletedCategory.Should().BeNull();
    }
    
    [Fact(DisplayName = nameof(GivenAValidId_whenCallsDeleteById_shouldThrowsANotFound))]
    [Trait("E2E/Api", "DeleteCategory - Endpoints")]
    public async Task GivenAValidId_whenCallsDeleteById_shouldThrowsANotFound()
    {
        //given
        var categoriesList = _fixture.GetValidCategoryList(20);
        await _fixture.Persistence.InsertList(categoriesList);
        
        var anId = Guid.NewGuid();
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Delete<ProblemDetails>($"/categories/{anId}");

        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status404NotFound);
        response.Should().NotBeNull();
        response!.Title.Should().Be("Not Found");
        response!.Type.Should().Be("NotFound");
        response!.Status.Should().Be(StatusCodes.Status404NotFound);
        response!.Detail.Should().Be($"Category '{anId}' not found.");
    }
    
}