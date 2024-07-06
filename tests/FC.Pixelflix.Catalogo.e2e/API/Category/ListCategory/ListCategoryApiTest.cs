using System.Net;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.ListCategory;

[Collection(nameof(ListCategoryApiTestFixtureCollection))]
public class ListCategoryApiTest : IDisposable
{
    private readonly ListCategoryApiTestFixture _fixture;
    
    public ListCategoryApiTest(ListCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GivenAValidRequest_whenCallsListCategories_shouldReturnAListOfCategories))]
    [Trait("E2E/Api", "ListCategory - Endpoints")]
    public async Task GivenAValidRequest_whenCallsListCategories_shouldReturnAListOfCategories()
    {
        var expectedTotalItems = 20;
        var expectedPerPage = 15;
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
    
    [Fact(DisplayName = nameof(GivenAValidRequest_whenCallsListCategoriesAndTheresNoPersistence_shouldReturnAEmptyList))]
    [Trait("E2E/Api", "ListCategory - Endpoints")]
    public async Task GivenAValidRequest_whenCallsListCategoriesAndTheresNoPersistence_shouldReturnAEmptyList()
    {
        //given
        
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Get<ListCategoriesResponse>($"/categories");

        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Items.Should().HaveCount(0);
        response.Total.Should().Be(0);
    }
    
    [Fact(DisplayName = nameof(GivenAValidRequest_whenCallsListCategories_shouldReturnAListOfCategories))]
    [Trait("E2E/Api", "ListCategory - Endpoints")]
    public async Task GivenAValidRequest_whenCallsListCategoriesWithPagination_shouldReturnAListOfCategoriesPaginated()
    {
        //given
        var expectedPage = 1;
        var expectedPerPage = 5;
        var expectedTotalItems = 20;
        
        var categoriesList = _fixture.GetValidCategoryList(expectedTotalItems);
        await _fixture.Persistence.InsertList(categoriesList);
        var requestPaginated = new ListCategoriesRequest(page: expectedPage, perPage: expectedPerPage);
        
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Get<ListCategoriesResponse>($"/categories", requestPaginated);

        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Items.Should().HaveCount(expectedPerPage);
        response.Page.Should().Be(expectedPage);
        response.PerPage.Should().Be(expectedPerPage);
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

    public void Dispose() => _fixture.CleanDatabase();
}