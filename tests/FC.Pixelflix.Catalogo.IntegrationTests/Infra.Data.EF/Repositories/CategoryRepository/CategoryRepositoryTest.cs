using FluentAssertions;
using Xunit;
using FC.Pixelflix.Catalogo.Infra.Data.EF;
using Repository = FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;
using FC.Pixelflix.Catalogo.Application.Exceptions;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using FC.Pixelflix.Catalogo.Domain.Entities;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestFixtureCollection))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture categoryRepositoryTestFixture)
    {
        _fixture = categoryRepositoryTestFixture;
    }

    [Fact(DisplayName = "CategoryRepository Integration INSERT test")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task givenAValidCategory_whenCallsInsert_shouldBeOk()
    {
        //Given
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var aCategory = _fixture.GetValidCategory();

        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        //When
        await aCategoryRepository.Insert(aCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        PixelflixCatalogDbContext aSecondContext = _fixture.CreateDbContext(true);
        var dbCategory = await aSecondContext.Categories.FindAsync(aCategory.Id);

        //Then
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(aCategory.Name);
        dbCategory.Description.Should().Be(aCategory.Description);
        dbCategory.IsActive.Should().Be(aCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(aCategory.CreatedAt);
    }

    [Fact(DisplayName = "CategoryRepository Integration GET test")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task givenAValidCategory_whenCallsGet_shouldBeOk()
    {
        //Given
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var aCategory = _fixture.GetValidCategory();
        var categoriesList = _fixture.GetValidCategoryList(15);
        categoriesList.Add(aCategory);
        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        //When

        var dbCategory = await aCategoryRepository.Get(aCategory.Id, CancellationToken.None);

        //Then
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(aCategory.Id);
        dbCategory.Name.Should().Be(aCategory.Name);
        dbCategory.Description.Should().Be(aCategory.Description);
        dbCategory.IsActive.Should().Be(aCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(aCategory.CreatedAt);
    }

    [Fact(DisplayName = "CategoryRepository Integration INVALID GET test")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task givenAValidCategory_whenCallsGetWithAIdNotPresent_shouldReturnNotFound()
    {
        //Given
        var anId = Guid.NewGuid();
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categoriesList = _fixture.GetValidCategoryList(15);
        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        //When

        var aTask = async () => await aCategoryRepository.Get(anId, CancellationToken.None);

        //Then
        await aTask.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{anId}' not found.");
    }

    [Fact(DisplayName = "CategoryRepository Integration UPDATE test")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task givenAValidCategory_whenCallsUpdate_shouldBeOk()
    {
        //Given
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var aCategory = _fixture.GetValidCategory();
        var aUpdatedCategory = _fixture.GetValidCategory();
        var categoriesList = _fixture.GetValidCategoryList(15);
        categoriesList.Add(aCategory);
        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        aCategory.Update(aUpdatedCategory.Name, aUpdatedCategory.Description);

        //When
        await aCategoryRepository.Update(aCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        PixelflixCatalogDbContext aSecondContext = _fixture.CreateDbContext(true);
        var dbCategory = await aSecondContext.Categories.FindAsync(aCategory.Id);

        //Then
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(aCategory.Id);
        dbCategory.Name.Should().Be(aUpdatedCategory.Name);
        dbCategory.Description.Should().Be(aUpdatedCategory.Description);
        dbCategory.IsActive.Should().Be(aCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(aCategory.CreatedAt);
    }

    [Fact(DisplayName = "CategoryRepository Integration DELETE test")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task givenAValidCategory_whenCallsDelete_shouldBeOk()
    {
        //Given
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var aCategory = _fixture.GetValidCategory();
        var aUpdatedCategory = _fixture.GetValidCategory();
        var categoriesList = _fixture.GetValidCategoryList(15);
        categoriesList.Add(aCategory);
        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        //When
        await aCategoryRepository.Delete(aCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        PixelflixCatalogDbContext aSecondContext = _fixture.CreateDbContext(true);
        var dbCategory = await aSecondContext.Categories.FindAsync(aCategory.Id);

        //Then
        dbCategory.Should().BeNull();
    }

    [Fact(DisplayName = "CategoryRepository Integration Search test with total")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task givenAValidParams_whenCallsSearch_shouldReturnTotal()
    {
        //Given
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categoriesList = _fixture.GetValidCategoryList(15);
        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var searchRequest = new SearchRepositoryRequest(1, 20, "", "", SearchOrder.Asc);

        //When
        var categoryListResponse = await aCategoryRepository.Search(searchRequest, CancellationToken.None);

        //Then
        categoryListResponse.Should().NotBeNull();
        categoryListResponse.Items.Should().NotBeNull();
        categoryListResponse.CurrentPage.Should().Be(searchRequest.Page);
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

    [Fact(DisplayName = "CategoryRepository Integration Search test - Empty results should return empty")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task givenAValidParams_whenCallsSearchAndEmptyTotal_shouldReturnEmpty()
    {
        //Given
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        var searchRequest = new SearchRepositoryRequest(1, 20, "", "", SearchOrder.Asc);

        //When
        var categoryListResponse = await aCategoryRepository.Search(searchRequest, CancellationToken.None);

        //Then
        categoryListResponse.Should().NotBeNull();
        categoryListResponse.Items.Should().NotBeNull();
        categoryListResponse.CurrentPage.Should().Be(searchRequest.Page);
        categoryListResponse.PerPage.Should().Be(searchRequest.PerPage);
        categoryListResponse.Total.Should().Be(0);
        categoryListResponse.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = "CategoryRepository Integration Search test with page param")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
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
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categoriesList = _fixture.GetValidCategoryList(totalCategoriesGenerated);
        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var searchRequest = new SearchRepositoryRequest(page, perPage, "", "", SearchOrder.Asc);

        //When
        var categoryListResponse = await aCategoryRepository.Search(searchRequest, CancellationToken.None);

        //Then
        categoryListResponse.Should().NotBeNull();
        categoryListResponse.Items.Should().NotBeNull();
        categoryListResponse.CurrentPage.Should().Be(searchRequest.Page);
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

    [Theory(DisplayName = "CategoryRepository Integration Search Test with Text params")]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
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
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categoriesNames = new List<string>() { "Action", "Horror", "Horror - Real Facts" , "Drama", "Sci-fi", "Sci-fi IA", "Sci-fi Space" };
        var categoriesList = _fixture.GetValidCategoryListWithNames(categoriesNames);
        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var searchRequest = new SearchRepositoryRequest(page, perPage, searchTextParam, "", SearchOrder.Asc);

        //When
        var categoryListResponse = await aCategoryRepository.Search(searchRequest, CancellationToken.None);

        //Then
        categoryListResponse.Should().NotBeNull();
        categoryListResponse.Items.Should().NotBeNull();
        categoryListResponse.CurrentPage.Should().Be(searchRequest.Page);
        categoryListResponse.PerPage.Should().Be(searchRequest.PerPage);
        categoryListResponse.Total.Should().Be(expetedTotalItemsQuantity);
        categoryListResponse.Items.Should().HaveCount(expetedItemsQuantityReturned);

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
        PixelflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categoriesList = _fixture.GetValidCategoryList(10);
        var aCategoryRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(categoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var searchRequest = new SearchRepositoryRequest(1, 20, "", orderBy, searchOrder);

        //When
        var categoryListResponse = await aCategoryRepository.Search(searchRequest, CancellationToken.None);

        //Then
        
        var expectedOrderedList = _fixture.CloneCategoryListListAndOrderIt(categoriesList, orderBy, searchOrder);
        
        categoryListResponse.Should().NotBeNull();
        categoryListResponse.Items.Should().NotBeNull();
        categoryListResponse.CurrentPage.Should().Be(searchRequest.Page);
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
