using System.Net;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.e2e.API.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.GetCategoryById;

[CollectionDefinition(nameof(GetCategoryApiTestFixtureCollection))]
public class GetCategoryApiTestFixtureCollection : ICollectionFixture<GetCategoryApiTestFixture> { }

public class GetCategoryApiTestFixture : CategoryBaseFixture
{
    private readonly GetCategoryApiTestFixture _fixture;
    
    public GetCategoryApiTestFixture(GetCategoryApiTestFixture fixture)
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
        responseMessage.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Id.Should().Be(aCategory.Id);
        response.Name.Should().Be(aCategory.Name);
        response.Description.Should().Be(aCategory.Description);
        response.IsActive.Should().Be(aCategory.IsActive);
        response.CreatedAt.Should().Be(aCategory.CreatedAt);
    }
    
}