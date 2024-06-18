using FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
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
}