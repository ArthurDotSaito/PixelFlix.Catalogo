using FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using FluentAssertions;
using Moq;
using Xunit;
using DomainEntity = FC.Pixelflix.Catalogo.Domain.Entities;
using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Genre.ListGenres;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.ListGenre;

[Collection(nameof(ListGenreTestFixture))]
public class ListGenreTest
{
    private readonly ListGenreTestFixture _fixture;

    public ListGenreTest(ListGenreTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = nameof(GivenAValidCommand_whenCallsListGenre_shouldReturnAListOfGenres))]
    [Trait("Application", "ListGenre - Use Cases")]
    public async Task GivenAValidCommand_whenCallsListGenre_shouldReturnAListOfGenres()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var aGenreList = _fixture.GetValidGenreList();
        
        var useCase = new UseCase.ListGenres(genreRepositoryMock.Object);

        var input = _fixture.GetValidListGenreRequest();
        
        var repositoryResponse = new SearchRepositoryResponse<DomainEntity.Genre>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<DomainEntity.Genre>)aGenreList,
            total: new Random().Next(50, 200)
        );
        
        genreRepositoryMock.Setup(x => x.Search(It.IsAny<SearchRepositoryRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(repositoryResponse);
        
        var response = await useCase.Handle(input, CancellationToken.None);
        
        response.Should().NotBeNull();
        response.Page.Should().Be(repositoryResponse.CurrentPage);
        response.PerPage.Should().Be(repositoryResponse.PerPage);
        response.Total.Should().Be(repositoryResponse.Total);
        response.Items.Should().HaveCount(repositoryResponse.Items.Count);
        ((List<GenreModelResponse>)response.Items).ForEach(item =>
        {
            var aGenre = repositoryResponse.Items.FirstOrDefault(a => a.Id == item.Id);
            item.Should().NotBeNull();
            item.Name.Should().Be(aGenre!.Name);
            item.IsActive.Should().Be(aGenre.IsActive);
            item.CreatedAt.Should().Be(aGenre.CreatedAt);
        });
    }
    
    [Fact(DisplayName = nameof(GivenAListGenreCommand_whenEmpty_shouldReturnAEmptyList))]
    [Trait("Application", "ListGenre - Use Cases")]
    public async Task GivenAListGenreCommand_whenEmpty_shouldReturnAEmptyList()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        
        var useCase = new UseCase.ListGenres(genreRepositoryMock.Object);

        var input = _fixture.GetValidListGenreRequest();
        
        var repositoryResponse = new SearchRepositoryResponse<DomainEntity.Genre>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: new List<DomainEntity.Genre>(),
            total: new Random().Next(50, 200)
        );
        
        genreRepositoryMock.Setup(x => x.Search(It.IsAny<SearchRepositoryRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(repositoryResponse);
        
        var response = await useCase.Handle(input, CancellationToken.None);
        
        response.Should().NotBeNull();
        response.Page.Should().Be(repositoryResponse.CurrentPage);
        response.PerPage.Should().Be(repositoryResponse.PerPage);
        response.Total.Should().Be(repositoryResponse.Total);
        response.Items.Should().HaveCount(repositoryResponse.Items.Count);
    }
    
    [Fact(DisplayName = nameof(GivenAListGenreCommand_whenNoParams_shouldReturnAEmptyListWithDefaultValues))]
    [Trait("Application", "ListGenre - Use Cases")]
    public async Task GivenAListGenreCommand_whenNoParams_shouldReturnAEmptyListWithDefaultValues()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        
        var useCase = new UseCase.ListGenres(genreRepositoryMock.Object);
        
        var repositoryResponse = new SearchRepositoryResponse<DomainEntity.Genre>(
            currentPage: 1,
            perPage: 15,
            items: new List<DomainEntity.Genre>(),
            total: 0
        );
        
        genreRepositoryMock.Setup(x => x.Search(It.IsAny<SearchRepositoryRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(repositoryResponse);
        
        var response = await useCase.Handle(new ListGenresRequest(), CancellationToken.None);
        
        genreRepositoryMock.Verify(x =>x.Search(It.Is<SearchRepositoryRequest>(
            searchRequest => 
                searchRequest.Page == 1 && 
                searchRequest.PerPage == 15 &&
                searchRequest.Search == "" &&
                searchRequest.OrderBy == "" &&
                searchRequest.Order == SearchOrder.Asc),
            It.IsAny<CancellationToken>()));
            
    }
}