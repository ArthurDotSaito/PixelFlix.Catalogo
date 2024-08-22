using System.Net;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using FC.Pixelflix.Catalogo.e2e.Extensions.DateTime;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;
using Xunit.Abstractions;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.ListCategory;

class CategoryListResponse
{
    public IReadOnlyList<CategoryModelResponse> Data { get; private set; }
    public Meta Meta { get; private set; }
}

class Meta
{
    public Meta(int page, int perPage, int total)
    {
        CurrentPage = page;
        PerPage = perPage;
        Total = total;
    }

    public int CurrentPage { get; private set; }
    public int PerPage { get; private set; }
    public int Total { get; private set; }
}

[Collection(nameof(ListCategoryApiTestFixtureCollection))]
public class ListCategoryApiTest : IDisposable
{
    private readonly ListCategoryApiTestFixture _fixture;
    private readonly ITestOutputHelper _output;
    
    public ListCategoryApiTest(ListCategoryApiTestFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
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
        var (responseMessage, response) = await _fixture.ApiClient.Get<CategoryListResponse>($"/categories");

        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Data.Should().NotBeNull();
        response.Meta.Should().NotBeNull();
        response!.Data.Should().HaveCount(expectedPerPage);
        response.Meta.Total.Should().Be(categoriesList.Count);
        response.Meta.PerPage.Should().Be(expectedPerPage);
        response.Meta.CurrentPage.Should().Be(1);

        foreach (var category in response.Data)
        {
            var expectedItem = categoriesList.FirstOrDefault(x => x.Id == category.Id);

            expectedItem.Should().NotBeNull();
            category.Name.Should().Be(expectedItem!.Name);
            category.Description.Should().Be(expectedItem.Description);
            category.IsActive.Should().Be(expectedItem.IsActive);
            category.CreatedAt.TrimMilliseconds().Should().Be(expectedItem.CreatedAt.TrimMilliseconds());
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
            category.CreatedAt.TrimMilliseconds().Should().Be(expectedItem.CreatedAt.TrimMilliseconds());
        }
    }
    
    [Theory(DisplayName = nameof(GivenAValidRequest_whenCallsListCategoriesWithPaginationAndItemsPerPage_shouldReturnAListOfCategoriesPaginated))]
    [Trait("E2E/Api", "ListCategory - Endpoints")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task GivenAValidRequest_whenCallsListCategoriesWithPaginationAndItemsPerPage_shouldReturnAListOfCategoriesPaginated(
        int totalCategoriesGenerated,
        int page,
        int perPage,
        int expetedItemsQuantity)
    {
        //given
        var categoriesList = _fixture.GetValidCategoryList(totalCategoriesGenerated);
        await _fixture.Persistence.InsertList(categoriesList);
        var requestPaginated = new ListCategoriesRequest(page, perPage);
        
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Get<ListCategoriesResponse>($"/categories", requestPaginated);

        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Items.Should().HaveCount(expetedItemsQuantity);
        response.Page.Should().Be(page);
        response.PerPage.Should().Be(perPage);
        response.Total.Should().Be(categoriesList.Count);

        foreach (var category in response.Items)
        {
            var expectedItem = categoriesList.FirstOrDefault(x => x.Id == category.Id);

            expectedItem.Should().NotBeNull();
            category.Name.Should().Be(expectedItem!.Name);
            category.Description.Should().Be(expectedItem.Description);
            category.IsActive.Should().Be(expectedItem.IsActive);
            category.CreatedAt.TrimMilliseconds().Should().Be(expectedItem.CreatedAt.TrimMilliseconds());
        }
    }
    
    [Theory(DisplayName = nameof(GivenAValidRequest_whenCallsListCategoriesWithTextParams_shouldReturnAListOfCategoriesPaginated))]
    [Trait("E2E/Api", "ListCategory - Endpoints")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 2, 2)]
    [InlineData("Horror", 2, 5, 0, 2)]
    [InlineData("Sci-fi", 1, 5, 3, 3)]
    [InlineData("Sci-fi", 1, 2, 2, 3)]
    [InlineData("Sci-fi", 2, 2, 1, 3)]
    [InlineData("Documen", 1, 2, 0, 0)]
    [InlineData("IA", 1, 2, 1, 1)]
    public async Task GivenAValidRequest_whenCallsListCategoriesWithTextParams_shouldReturnAListOfCategoriesPaginated(
        string searchTextParam,
        int page,
        int perPage,
        int expetedItemsQuantityReturned,
        int expetedTotalItemsQuantity
        )
    {
        //given
        var categoriesNames = new List<string>() { "Action", "Horror", "Horror - Real Facts" , "Drama", "Sci-fi", "Sci-fi IA", "Sci-fi Space" };
        var categoriesList = _fixture.GetValidCategoryListWithNames(categoriesNames);
        await _fixture.Persistence.InsertList(categoriesList);
        var requestPaginated = new ListCategoriesRequest(page, perPage, searchTextParam);
        
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Get<ListCategoriesResponse>($"/categories", requestPaginated);

        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Items.Should().HaveCount(expetedItemsQuantityReturned);
        response.Page.Should().Be(page);
        response.PerPage.Should().Be(perPage);
        response.Total.Should().Be(expetedTotalItemsQuantity);

        foreach (var category in response.Items)
        {
            var expectedItem = categoriesList.FirstOrDefault(x => x.Id == category.Id);

            expectedItem.Should().NotBeNull();
            category.Name.Should().Be(expectedItem!.Name);
            category.Description.Should().Be(expectedItem.Description);
            category.IsActive.Should().Be(expectedItem.IsActive);
            category.CreatedAt.TrimMilliseconds().Should().Be(expectedItem.CreatedAt.TrimMilliseconds());
        }
    }
    
    [Theory(DisplayName = nameof(GivenAValidRequest_whenCallsListCategoriesWithOrdenation_shouldReturnAOrderedListOfCategories))]
    [Trait("E2E/Api", "ListCategory - Endpoints")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("", "asc")]
    public async Task GivenAValidRequest_whenCallsListCategoriesWithOrdenation_shouldReturnAOrderedListOfCategories(
        string orderBy,
        string order
        )
    {
        //given
        var expectedTotalItems = 10;
        var categoriesList = _fixture.GetValidCategoryList(expectedTotalItems);
        await _fixture.Persistence.InsertList(categoriesList);
        var useCaseOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var requestPaginated = new ListCategoriesRequest(page:1, perPage:20, sort: orderBy, dir: useCaseOrder);
        
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Get<ListCategoriesResponse>($"/categories", requestPaginated);

        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Items.Should().HaveCount(expectedTotalItems);
        response.Page.Should().Be(requestPaginated.Page);
        response.PerPage.Should().Be(requestPaginated.PerPage);
        response.Total.Should().Be(categoriesList.Count);
        
        var expectedOrderedList = _fixture.CloneCategoryListListAndOrderIt(categoriesList, requestPaginated.Sort, requestPaginated.Dir);

        for (int i = 0; i < expectedOrderedList.Count; i++)
        {
            var expectedItem = expectedOrderedList[i];
            var responseItem = response.Items[i];
            
            expectedItem.Should().NotBeNull();
            responseItem.Should().NotBeNull();
            responseItem.Name.Should().Be(expectedItem.Name);
            responseItem!.Id.Should().Be(expectedItem.Id);
            responseItem.Description.Should().Be(expectedItem.Description);
            responseItem.IsActive.Should().Be(expectedItem.IsActive);
            responseItem.CreatedAt.TrimMilliseconds().Should().Be(expectedItem.CreatedAt.TrimMilliseconds());   
        }
    }
    
    [Theory(DisplayName = nameof(GivenAValidRequest_whenCallsListCategoriesWithDateOrdenation_shouldReturnAOrderedListOfCategories))]
    [Trait("E2E/Api", "ListCategory - Endpoints")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    public async Task GivenAValidRequest_whenCallsListCategoriesWithDateOrdenation_shouldReturnAOrderedListOfCategories(
        string orderBy,
        string order
        )
    {
        //given
        var expectedTotalItems = 10;
        var categoriesList = _fixture.GetValidCategoryList(expectedTotalItems);
        await _fixture.Persistence.InsertList(categoriesList);
        var useCaseOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var requestPaginated = new ListCategoriesRequest(page:1, perPage:20, sort: orderBy, dir: useCaseOrder);
        
        //when
        var (responseMessage, response) = await _fixture.ApiClient.Get<ListCategoriesResponse>($"/categories", requestPaginated);

        //then
        responseMessage.Should().NotBeNull();
        responseMessage!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        response.Should().NotBeNull();
        response!.Items.Should().HaveCount(expectedTotalItems);
        response.Page.Should().Be(requestPaginated.Page);
        response.PerPage.Should().Be(requestPaginated.PerPage);
        response.Total.Should().Be(categoriesList.Count);

        DateTime? lastItemDate = null;

        foreach (var category in response.Items)
        {
            var expectedItem = categoriesList.FirstOrDefault(x => x.Id == category.Id);

            expectedItem.Should().NotBeNull();
            category.Name.Should().Be(expectedItem!.Name);
            category.Description.Should().Be(expectedItem.Description);
            category.IsActive.Should().Be(expectedItem.IsActive);
            category.CreatedAt.TrimMilliseconds().Should().Be(expectedItem.CreatedAt.TrimMilliseconds());
            
            if (lastItemDate != null && order == "asc") Assert.True(category.CreatedAt >= lastItemDate);
            else if(lastItemDate != null && order == "desc") Assert.True(category.CreatedAt <= lastItemDate);
            
            lastItemDate = category.CreatedAt;
        }
    }
    public void Dispose() => _fixture.CleanDatabase();
   
}