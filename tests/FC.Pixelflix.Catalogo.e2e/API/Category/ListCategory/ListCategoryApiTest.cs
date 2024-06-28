using System.Net;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.ListCategory;

[Collection(nameof(ListCategoryApiTestFixtureCollection))]
public class ListCategoryApiTest
{
    private readonly ListCategoryApiTestFixture _fixture;
    
    public ListCategoryApiTest(ListCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GivenAValidRequest_whenCallsListCategories_shouldReturnAListOfCategories))]
    [Trait("E2E/Api", "ListCategory - Endpoints")]
    public async void GivenAValidRequest_whenCallsListCategories_shouldReturnAListOfCategories()
    {
        var expectedTotalItems = 20;
        var expectedPerPage = 10;
        //given
        var categoriesList = _fixture.GetValidCategoryList(expectedTotalItems);
        await _fixture.Persistence.InsertList(categoriesList);
        
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Get<ListCategoriesResponse>($"/categories");

        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Items.Should().HaveCount(expectedPerPage);
        response.Total.Should().Be(categoriesList.Count);

        foreach (var category in response.Items)
        {
            var expectedItem = categoriesList.FirstOrDefault(x => x.Id == category.Id);

            expectedItem.Should().NotBeNull();
            category.Name.Should().Be(expectedItem!.Name);
            category.Description.Should().Be(expectedItem.Description);
            category.IsActive.Should().Be(expectedItem.IsActive);
            category.CreatedAt.Should().Be(expectedItem.CreatedAt);
        }
    }
    
}