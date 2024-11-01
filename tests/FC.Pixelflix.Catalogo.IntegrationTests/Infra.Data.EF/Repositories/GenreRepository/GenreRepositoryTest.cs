using FC.Pixelflix.Catalogo.Application.Exceptions;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using FC.Pixelflix.Catalogo.Infra.Data.EF;
using FC.Pixelflix.Catalogo.Infra.Data.EF.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Repository = FC.Pixelflix.Catalogo.Infra.Data.EF.Repositories;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Infra.Data.EF.Repositories.GenreRepository;

[Collection(nameof(GenreRepositoryTestFixtureCollection))]
public class GenreRepositoryTest
{
    private readonly GenreRepositoryTestFixture _fixture;

    public GenreRepositoryTest(GenreRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = "CategoryRepository Integration INSERT test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenAValidGenre_whenCallsInsert_shouldPersistAGenre()
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var aGenre = _fixture.GetValidGenre();
        var categories = _fixture.GetValidCategoryList(3);
        
        categories.ForEach(category => aGenre.AddCategory(category.Id));
        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
        
        var genreRepository = new Repository.GenreRepository(dbContext);

        //When
        await genreRepository.Insert(aGenre, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var assertsDbContext = _fixture.CreateDbContext(true);
        var dbGenres = await assertsDbContext.Genres.FindAsync(aGenre.Id);

        //Then
        dbGenres.Should().NotBeNull();
        dbGenres!.Name.Should().Be(aGenre.Name);
        dbGenres.IsActive.Should().Be(aGenre.IsActive);
        dbGenres.CreatedAt.Should().Be(aGenre.CreatedAt);
        
        var genresCategoryRelation = await assertsDbContext.GenresCategories.Where(gc => gc.GenreId == aGenre.Id).ToListAsync();
        
        genresCategoryRelation.Should().HaveCount(categories.Count);
        genresCategoryRelation.ForEach(relation =>
        {
            var expectedCategory = categories.FirstOrDefault(c => c.Id == relation.CategoryId);
            expectedCategory.Should().NotBeNull();
        });
    }
    
    [Fact(DisplayName = "CategoryRepository Integration GET test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenAGetCommand_whenTheresAGenre_shouldReturnAGenre()
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var aGenre = _fixture.GetValidGenre();
        var categories = _fixture.GetValidCategoryList(3);
        
        categories.ForEach(category => aGenre.AddCategory(category.Id));
        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.Genres.AddAsync(aGenre);

        foreach (var categoryId in aGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, aGenre.Id);
            await dbContext.GenresCategories.AddRangeAsync(relation);
        }
        
        await dbContext.SaveChangesAsync();
        
        var genreRepository = new Repository.GenreRepository(_fixture.CreateDbContext(true));

        //When
        var databaseGenre = await genreRepository.Get(aGenre.Id, CancellationToken.None);

        var assertsDbContext = _fixture.CreateDbContext(true);

        //Then
        databaseGenre.Should().NotBeNull();
        databaseGenre!.Name.Should().Be(aGenre.Name);
        databaseGenre.IsActive.Should().Be(aGenre.IsActive);
        databaseGenre.CreatedAt.Should().Be(aGenre.CreatedAt);
        databaseGenre.Categories.Should().HaveCount(categories.Count);

        foreach (var categoryId in databaseGenre.Categories)
        {
            var dbCategory = await assertsDbContext.Categories.FindAsync(categoryId);
            dbCategory.Should().NotBeNull();   
        }
    }
    
    [Fact(DisplayName = "CategoryRepository Integration GET test - Not Found")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenAGetCommand_whenTheresNoGenre_shouldThrowAError()
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var aGenre = _fixture.GetValidGenre();
        var categories = _fixture.GetValidCategoryList(3);
        
        categories.ForEach(category => aGenre.AddCategory(category.Id));
        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.Genres.AddAsync(aGenre);

        foreach (var categoryId in aGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, aGenre.Id);
            await dbContext.GenresCategories.AddRangeAsync(relation);
        }
        
        await dbContext.SaveChangesAsync();
        
        var genreRepository = new Repository.GenreRepository(_fixture.CreateDbContext(true));
        var ARandomId = Guid.NewGuid();
        
        //When
        var action = async () => await genreRepository.Get(ARandomId, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre '{ARandomId}' was not found");
    }
    
    [Fact(DisplayName = "CategoryRepository Integration DELETE test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenADeleteCommand_whenGenresExist_shouldDeleteAGenre()
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var aGenre = _fixture.GetValidGenre();
        var categories = _fixture.GetValidCategoryList(3);
        
        categories.ForEach(category => aGenre.AddCategory(category.Id));
        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.Genres.AddAsync(aGenre);

        foreach (var categoryId in aGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, aGenre.Id);
            await dbContext.GenresCategories.AddRangeAsync(relation);
        }
        
        await dbContext.SaveChangesAsync();
        var repositoryDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(repositoryDbContext);
    
        //When
        await genreRepository.Delete(aGenre, CancellationToken.None);
        await repositoryDbContext.SaveChangesAsync();

        var assertsDbContext = _fixture.CreateDbContext(true);
        var dbGenre = await assertsDbContext.Genres.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == aGenre.Id);

        //Then
        dbGenre.Should().BeNull();
        
        var genresCategoryRelation = await assertsDbContext.GenresCategories.Where(gc => gc.GenreId == aGenre.Id).ToListAsync();

        genresCategoryRelation.Should().HaveCount(0);
    }
    
    [Fact(DisplayName = "CategoryRepository Integration UPDATE test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenAUpdateCommand_whenGenresExist_shouldUpdateAGenre()
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var aGenre = _fixture.GetValidGenre();
        var categories = _fixture.GetValidCategoryList(3);
        
        categories.ForEach(category => aGenre.AddCategory(category.Id));
        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.Genres.AddAsync(aGenre);

        foreach (var categoryId in aGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, aGenre.Id);
            await dbContext.GenresCategories.AddRangeAsync(relation);
        }
        
        await dbContext.SaveChangesAsync();
        
        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);
        aGenre.Update(_fixture.GetValidGenreName());
        if(aGenre.IsActive) aGenre.Deactivate();
        else aGenre.Activate();
        
        //When
        await genreRepository.Update(aGenre, CancellationToken.None);
        await actDbContext.SaveChangesAsync();

        var assertsDbContext = _fixture.CreateDbContext(true);
        var dbGenre = await assertsDbContext.Genres.FindAsync(aGenre.Id);

        //Then
        dbGenre.Should().NotBeNull();
        dbGenre!.Name.Should().Be(aGenre.Name);
        dbGenre.IsActive.Should().Be(aGenre.IsActive);
        dbGenre.CreatedAt.Should().Be(aGenre.CreatedAt);
        
        var genresCategoryRelation = await assertsDbContext.GenresCategories.Where(gc => gc.GenreId == aGenre.Id).ToListAsync();
        
        genresCategoryRelation.Should().HaveCount(categories.Count);
        genresCategoryRelation.ForEach(relation =>
        {
            var expectedCategory = categories.FirstOrDefault(c => c.Id == relation.CategoryId);
            expectedCategory.Should().NotBeNull();
        });
    }
    
    [Fact(DisplayName = "CategoryRepository Integration UPDATE REMOVING RELATIONS test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenAUpdateCommand_whenRemovingAllCategories_shouldUpdateToZeroCategoriesRelated()
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var aGenre = _fixture.GetValidGenre();
        var categories = _fixture.GetValidCategoryList(3);
        
        categories.ForEach(category => aGenre.AddCategory(category.Id));
        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.Genres.AddAsync(aGenre);

        foreach (var categoryId in aGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, aGenre.Id);
            await dbContext.GenresCategories.AddRangeAsync(relation);
        }
        
        await dbContext.SaveChangesAsync();
        
        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);
        aGenre.Update(_fixture.GetValidGenreName());
        if(aGenre.IsActive) aGenre.Deactivate();
        else aGenre.Activate();
        
        aGenre.RemoveAllCategories();
        
        //When
        await genreRepository.Update(aGenre, CancellationToken.None);
        await actDbContext.SaveChangesAsync();

        var assertsDbContext = _fixture.CreateDbContext(true);
        var dbGenre = await assertsDbContext.Genres.FindAsync(aGenre.Id);

        //Then
        dbGenre.Should().NotBeNull();
        dbGenre!.Name.Should().Be(aGenre.Name);
        dbGenre.IsActive.Should().Be(aGenre.IsActive);
        dbGenre.CreatedAt.Should().Be(aGenre.CreatedAt);
        
        var genresCategoryRelation = await assertsDbContext.GenresCategories.Where(gc => gc.GenreId == aGenre.Id).ToListAsync();
        
        genresCategoryRelation.Should().HaveCount(0);
    }
    
    [Fact(DisplayName = "CategoryRepository Integration UPDATE REPLACING RELATIONS test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenAUpdateCommand_whenChangeRelations_shouldUpdateWithModifiedRelations()
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var aGenre = _fixture.GetValidGenre();
        var categories = _fixture.GetValidCategoryList(3);
        var newCategories = _fixture.GetValidCategoryList(5);
        
        categories.ForEach(category => aGenre.AddCategory(category.Id));
        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.Categories.AddRangeAsync(newCategories);
        await dbContext.Genres.AddAsync(aGenre);

        foreach (var categoryId in aGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, aGenre.Id);
            await dbContext.GenresCategories.AddRangeAsync(relation);
        }
        
        await dbContext.SaveChangesAsync();
        
        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);
        aGenre.Update(_fixture.GetValidGenreName());
        if(aGenre.IsActive) aGenre.Deactivate();
        else aGenre.Activate();
        
        aGenre.RemoveAllCategories();
        newCategories.ForEach(category => aGenre.AddCategory(category.Id));
        
        //When
        await genreRepository.Update(aGenre, CancellationToken.None);
        await actDbContext.SaveChangesAsync();

        var assertsDbContext = _fixture.CreateDbContext(true);
        var dbGenre = await assertsDbContext.Genres.FindAsync(aGenre.Id);

        //Then
        dbGenre.Should().NotBeNull();
        dbGenre!.Name.Should().Be(aGenre.Name);
        dbGenre.IsActive.Should().Be(aGenre.IsActive);
        dbGenre.CreatedAt.Should().Be(aGenre.CreatedAt);
        
        var genresCategoryRelation = await assertsDbContext.GenresCategories.Where(gc => gc.GenreId == aGenre.Id).ToListAsync();
        
        genresCategoryRelation.Should().HaveCount(newCategories.Count);
        genresCategoryRelation.ForEach(relation =>
        {
            var expectedCategory = newCategories.FirstOrDefault(c => c.Id == relation.CategoryId);
            expectedCategory.Should().NotBeNull();
        });
    }
    
    [Fact(DisplayName = "CategoryRepository Integration LIST test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenASearchCommand_whenCalled_shouldReturnAGenreList()
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var aGenreList = _fixture.GetValidGenreList();
        
        await dbContext.Genres.AddRangeAsync(aGenreList);
        await dbContext.SaveChangesAsync();
        
        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);

        var searchRequest = new SearchRepositoryRequest(1, 20, "", "", SearchOrder.Asc);
        //When
        var searchResponse = await genreRepository.Search(searchRequest, CancellationToken.None);
        await actDbContext.SaveChangesAsync();

        //Then
        searchResponse.Should().NotBeNull();
        searchResponse.CurrentPage.Should().Be(searchRequest.Page);
        searchResponse.PerPage.Should().Be(searchRequest.PerPage);
        searchResponse.Total.Should().Be(aGenreList.Count);
        searchResponse.Items.Should().HaveCount(aGenreList.Count);

        foreach (var item in searchResponse.Items)
        {
            var exampleGenre = aGenreList.Find(e => e.Id == item.Id);
            item.Should().NotBeNull();
            item.Name.Should().Be(exampleGenre.Name);
            item.IsActive.Should().Be(exampleGenre.IsActive);
            item.CreatedAt.Should().Be(exampleGenre.CreatedAt);
        }
    }
    
    [Fact(DisplayName = "CategoryRepository Integration LIST test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenASearchCommand_whenTheresRelations_shouldReturnAGenreWithRelations()
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var aGenreList = _fixture.GetValidGenreList();
        
        await dbContext.Genres.AddRangeAsync(aGenreList);
        
        aGenreList.ForEach(genre =>
        {
            var categoriesListRelation = _fixture.GetValidCategoryList(new Random().Next(0, 4));
            if(categoriesListRelation.Count > 0)
            {
                categoriesListRelation.ForEach(category => genre.AddCategory(category.Id));
                dbContext.Categories.AddRange(categoriesListRelation);
                var relations = categoriesListRelation.Select(category => new GenresCategories(category.Id, genre.Id));
                dbContext.GenresCategories.AddRange(relations);
            }
        });
        
        await dbContext.SaveChangesAsync();
        
        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);

        var searchRequest = new SearchRepositoryRequest(1, 20, "", "", SearchOrder.Asc);
        //When
        var searchResponse = await genreRepository.Search(searchRequest, CancellationToken.None);
        await actDbContext.SaveChangesAsync();

        //Then
        searchResponse.Should().NotBeNull();
        searchResponse.CurrentPage.Should().Be(searchRequest.Page);
        searchResponse.PerPage.Should().Be(searchRequest.PerPage);
        searchResponse.Total.Should().Be(aGenreList.Count);
        searchResponse.Items.Should().HaveCount(aGenreList.Count);

        foreach (var item in searchResponse.Items)
        {
            var exampleGenre = aGenreList.Find(e => e.Id == item.Id);
            item.Should().NotBeNull();
            item.Name.Should().Be(exampleGenre.Name);
            item.IsActive.Should().Be(exampleGenre.IsActive);
            item.CreatedAt.Should().Be(exampleGenre.CreatedAt);
            item.Categories.Should().BeEquivalentTo(exampleGenre.Categories);
            exampleGenre.Categories.Should().HaveCount(item.Categories.Count);
        }
    }
    
    [Fact(DisplayName = "CategoryRepository Integration EMPTY GENRE test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task givenASearchCommand_whenThereIsNoGenre_shouldReturnEmpty()
    {
        //Given
        
        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);

        var searchRequest = new SearchRepositoryRequest(1, 20, "", "", SearchOrder.Asc);
        //When
        var searchResponse = await genreRepository.Search(searchRequest, CancellationToken.None);
        await actDbContext.SaveChangesAsync();

        //Then
        searchResponse.Should().NotBeNull();
        searchResponse.CurrentPage.Should().Be(searchRequest.Page);
        searchResponse.PerPage.Should().Be(searchRequest.PerPage);
        searchResponse.Total.Should().Be(0);
        searchResponse.Items.Should().HaveCount(0);
    }
    
    [Theory(DisplayName = "CategoryRepository Integration LIST PAGINATED test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    [InlineData(10,1,5,5)]
    [InlineData(10,2,5,5)]
    [InlineData(7,2,5,2)]
    [InlineData(7,3,5,0)]
    public async Task givenASearchCommandWithPagination_whenCalled_shouldReturnAGenreListPaginated(
        int totalGenres,
        int page,
        int perPage,
        int expectedCount)
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var aGenreList = _fixture.GetValidGenreList(totalGenres);
        
        await dbContext.Genres.AddRangeAsync(aGenreList);
        await dbContext.SaveChangesAsync();
        
        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);

        var searchRequest = new SearchRepositoryRequest(page, perPage, "", "", SearchOrder.Asc);
        //When
        var searchResponse = await genreRepository.Search(searchRequest, CancellationToken.None);
        await actDbContext.SaveChangesAsync();

        //Then
        searchResponse.Should().NotBeNull();
        searchResponse.CurrentPage.Should().Be(searchRequest.Page);
        searchResponse.PerPage.Should().Be(searchRequest.PerPage);
        searchResponse.Total.Should().Be(totalGenres);
        searchResponse.Items.Should().HaveCount(expectedCount);
    }
    
    [Theory(DisplayName = "CategoryRepository Integration LIST PAGINATED WITH TEXT FILTER test")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 2, 2)]
    [InlineData("Horror", 2, 5, 0, 2)]
    [InlineData("Sci-fi", 1, 5, 3, 3)]
    [InlineData("Sci-fi", 1, 2, 2, 3)]
    [InlineData("Sci-fi", 2, 2, 1, 3)]
    [InlineData("Documen", 1, 2, 0, 0)]
    [InlineData("IA", 1, 2, 1, 1)]
    public async Task givenASearchCommand_whenCalledWithTextParam_shouldReturnAGenreListPaginatedByText(
        string search,
        int page,
        int perPage,
        int expectedItemsCount,
        int expectedTotalCount)
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var someGenreNames = new List<string>() { "Action", "Horror", "Horror - Real Facts" , "Drama", "Sci-fi", "Sci-fi IA", "Sci-fi Space" };
        var genreList = _fixture.GetValidGenreListWithNames(someGenreNames);
        
        await dbContext.Genres.AddRangeAsync(genreList);
        
        genreList.ForEach(genre =>
        {
            var categoriesListRelation = _fixture.GetValidCategoryList(new Random().Next(0, 4));
            if(categoriesListRelation.Count > 0)
            {
                categoriesListRelation.ForEach(category => genre.AddCategory(category.Id));
                dbContext.Categories.AddRange(categoriesListRelation);
                var relations = categoriesListRelation.Select(category => new GenresCategories(category.Id, genre.Id));
                dbContext.GenresCategories.AddRange(relations);
            }
        });
        await dbContext.SaveChangesAsync();
        
        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);

        var searchRequest = new SearchRepositoryRequest(page, perPage, search, "", SearchOrder.Asc);
        //When
        var searchResponse = await genreRepository.Search(searchRequest, CancellationToken.None);
        await actDbContext.SaveChangesAsync();

        //Then
        searchResponse.Should().NotBeNull();
        searchResponse.CurrentPage.Should().Be(searchRequest.Page);
        searchResponse.PerPage.Should().Be(searchRequest.PerPage);
        searchResponse.Total.Should().Be(expectedTotalCount);
        searchResponse.Items.Should().HaveCount(expectedItemsCount);
    }
    
     [Theory(DisplayName = "CategoryRepository Integration Search Test with order params")]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    [InlineData("", "asc")]
    public async Task givenAValidCommand_whenCallsSearchWithOrderParams_shouldReturnGenresOrdered(
        string orderBy,
        string order
    )
    {
        //Given
        var dbContext = _fixture.CreateDbContext();
        var genreList = _fixture.GetValidGenreList(10);
        var aGenreRepository = new Repository.CategoryRepository(dbContext);

        await dbContext.AddRangeAsync(genreList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var searchRequest = new SearchRepositoryRequest(1, 20, "", orderBy, searchOrder);

        //When
        var genreListResponse = await aGenreRepository.Search(searchRequest, CancellationToken.None);

        //Then
        
        var expectedOrderedList = _fixture.CloneGenreListListAndOrderIt(genreList, orderBy, searchOrder);
        
        genreListResponse.Should().NotBeNull();
        genreListResponse.Items.Should().NotBeNull();
        genreListResponse.CurrentPage.Should().Be(searchRequest.Page);
        genreListResponse.PerPage.Should().Be(searchRequest.PerPage);
        genreListResponse.Total.Should().Be(categoriesList.Count);
        genreListResponse.Items.Should().HaveCount(categoriesList.Count);
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