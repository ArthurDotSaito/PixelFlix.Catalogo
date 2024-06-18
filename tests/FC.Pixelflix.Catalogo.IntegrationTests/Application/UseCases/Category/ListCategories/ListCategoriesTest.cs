using FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
using FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;
using FluentAssertions;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.ListCategories;

[Collection(nameof(ListCategoriesTestFixtureColllection))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;
    
    public ListCategoriesTest(ListCategoriesTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = "Category List/Search Integration Search test with total")]
    [Trait("Integration/Application", "ListCategories - UseCases")]
    public async Task givenAValidParams_whenCallsSearch_shouldReturnTotal()
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var categoriesList = _fixture.GetValidCategoryList(10);
        var aCategoryRepository = new CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var useCase = new UseCase.ListCategories(aCategoryRepository);

        var searchRequest = new ListCategoriesRequest(1, 20);

        //When
        var categoryListResponse = await useCase.Handle(searchRequest, CancellationToken.None);

        //Then
        categoryListResponse.Should().NotBeNull();
        categoryListResponse.Items.Should().NotBeNull();
        categoryListResponse.Page.Should().Be(searchRequest.Page);
        categoryListResponse.PerPage.Should().Be(searchRequest.PerPage);
        categoryListResponse.Total.Should().Be(categoriesList.Count());
        categoryListResponse.Items.Should().HaveCount(categoriesList.Count);

        foreach (var category in categoriesList)
        {
            var aItem = categoriesList.Find(item => item.Id == category.Id);
            aItem.Should().NotBeNull();

            category!.Id.Should().Be(aItem.Id);
            category.Name.Should().Be(aItem.Name);
            category.Description.Should().Be(aItem.Description);
            category.IsActive.Should().Be(aItem.IsActive);
            category.CreatedAt.Should().Be(aItem.CreatedAt);
        }
    }
    
    [Fact(DisplayName = "Category List/Search Integration Search - Empty results should return empty")]
    [Trait("Integration/Application", "ListCategories - UseCases")]
    public async Task givenAValidParams_whenCallsSearchAndEmptyTotal_shouldReturnEmpty()
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var aCategoryRepository = new CategoryRepository(dbContext);

        var searchRequest = new ListCategoriesRequest(1, 20);
        var useCase = new UseCase.ListCategories(aCategoryRepository);
        //When
        var categoryListResponse = await useCase.Handle(searchRequest, CancellationToken.None);

        //Then
        categoryListResponse.Should().NotBeNull();
        categoryListResponse.Items.Should().NotBeNull();
        categoryListResponse.Page.Should().Be(searchRequest.Page);
        categoryListResponse.PerPage.Should().Be(searchRequest.PerPage);
        categoryListResponse.Total.Should().Be(0);
        categoryListResponse.Items.Should().HaveCount(0);
    }
    
    [Theory(DisplayName = "Category List/Search Integration Search test with page param")]
    [Trait("Integration/Application", "ListCategories - UseCases")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task givenAValidParams_whenCallsSearchWithPagination_shouldReturnTotalPaginated(
        int totalCategoriesGenerated,
        int page,
        int perPage,
        int expetedItemsQuantity
        )
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var categoriesList = _fixture.GetValidCategoryList(totalCategoriesGenerated);
        var aCategoryRepository = new CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var searchRequest = new ListCategoriesRequest(page, perPage);
        var useCase = new UseCase.ListCategories(aCategoryRepository);
        
        //When
        var categoryListResponse = await useCase.Handle(searchRequest, CancellationToken.None);

        //Then
        categoryListResponse.Should().NotBeNull();
        categoryListResponse.Items.Should().NotBeNull();
        categoryListResponse.Page.Should().Be(searchRequest.Page);
        categoryListResponse.PerPage.Should().Be(searchRequest.PerPage);
        categoryListResponse.Total.Should().Be(totalCategoriesGenerated);
        categoryListResponse.Items.Should().HaveCount(expetedItemsQuantity);

        foreach (var category in categoriesList)
        {
            var aItem = categoriesList.Find(item => item.Id == category.Id);
            aItem.Should().NotBeNull();

            category!.Id.Should().Be(aItem.Id);
            category.Name.Should().Be(aItem.Name);
            category.Description.Should().Be(aItem.Description);
            category.IsActive.Should().Be(aItem.IsActive);
            category.CreatedAt.Should().Be(aItem.CreatedAt);
        }
    }
    
    [Theory(DisplayName = "Category List/Search Integration Search test with Text param")]
    [Trait("Integration/Application", "ListCategories - UseCases")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 2, 2)]
    [InlineData("Horror", 2, 5, 0, 2)]
    [InlineData("Sci-fi", 1, 5, 3, 3)]
    [InlineData("Sci-fi", 1, 2, 2, 3)]
    [InlineData("Sci-fi", 2, 2, 1, 3)]
    [InlineData("Documen", 1, 2, 0, 0)]
    [InlineData("IA", 1, 2, 1, 1)]
    public async Task givenAValidCommand_whenCallsSearchWithText_shouldReturnCategories(
        string searchTextParam,
        int page,
        int perPage,
        int expetedItemsQuantityReturned,
        int expetedTotalItemsQuantity
    )
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var categoriesNames = new List<string>() { "Action", "Horror", "Horror - Real Facts" , "Drama", "Sci-fi", "Sci-fi IA", "Sci-fi Space" };
        var categoriesList = _fixture.GetValidCategoryListWithNames(categoriesNames);
        var aCategoryRepository = new CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var useCase = new UseCase.ListCategories(aCategoryRepository);
        var searchRequest = new ListCategoriesRequest(page, perPage, searchTextParam);

        //When
        var categoryListResponse = await useCase.Handle(searchRequest, CancellationToken.None);

        //Then
        categoryListResponse.Should().NotBeNull();
        categoryListResponse.Items.Should().NotBeNull();
        categoryListResponse.Page.Should().Be(searchRequest.Page);
        categoryListResponse.PerPage.Should().Be(searchRequest.PerPage);
        categoryListResponse.Total.Should().Be(expetedTotalItemsQuantity);
        categoryListResponse.Items.Should().HaveCount(expetedItemsQuantityReturned);

        foreach (var category in categoriesList)
        {
            var aItem = categoriesList.Find(item => item.Id == category.Id);
            aItem.Should().NotBeNull();

            category!.Id.Should().Be(aItem!.Id);
            category.Name.Should().Be(aItem.Name);
            category.Description.Should().Be(aItem.Description);
            category.IsActive.Should().Be(aItem.IsActive);
            category.CreatedAt.Should().Be(aItem.CreatedAt);
        }
    }
    
        [Theory(DisplayName = "CategoryRepository Integration Search Test with ordenation params")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    [InlineData("", "asc")]
    public async Task givenAValidCommand_whenCallsSearchWithOrdenationAsc_shouldReturnCategories(
        string orderBy,
        string order
    )
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var categoriesList = _fixture.GetValidCategoryList(10);
        var aCategoryRepository = new CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var useCase = new UseCase.ListCategories(aCategoryRepository);
        var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var searchRequest = new ListCategoriesRequest(1, 20, "", orderBy, searchOrder);

        //When
        var categoryListResponse = await useCase.Handle(searchRequest, CancellationToken.None);

        //Then
        var expectedOrderedList = _fixture.CloneCategoryListListAndOrderIt(categoriesList, orderBy, searchOrder);
        
        categoryListResponse.Should().NotBeNull();
        categoryListResponse.Items.Should().NotBeNull();
        categoryListResponse.Page.Should().Be(searchRequest.Page);
        categoryListResponse.PerPage.Should().Be(searchRequest.PerPage);
        categoryListResponse.Total.Should().Be(categoriesList.Count);
        categoryListResponse.Items.Should().HaveCount(categoriesList.Count);
        for (int i = 0; i < expectedOrderedList.Count; i++)
        {
            var expectedItem = expectedOrderedList[i];
            var responseItem = categoryListResponse.Items[i];
            
            expectedItem.Should().NotBeNull();
            responseItem.Should().NotBeNull();
            responseItem!.Id.Should().Be(expectedItem.Id);
            responseItem.Name.Should().Be(expectedItem.Name);
            responseItem.Description.Should().Be(expectedItem.Description);
            responseItem.IsActive.Should().Be(expectedItem.IsActive);
            responseItem.CreatedAt.Should().Be(expectedItem.CreatedAt);   
        }
    }
}