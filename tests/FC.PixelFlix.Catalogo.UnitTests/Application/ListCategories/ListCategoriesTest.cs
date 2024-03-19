using FC.Pixelflix.Catalogo.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.ListCategories;

[Collection (nameof(ListCategoriesTestCollection))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;

    public ListCategoriesTest(ListCategoriesTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GivenAValidRequest_WhenCallsListCategories_ShouldReturnOk))]
    [Trait("Application","ListCategories - UseCases")]
    public async Task GivenAValidRequest_WhenCallsListCategories_ShouldReturnOk()
    {
        //given
        var aCategoryList = _fixture.GetValidCategoryList();
        var aRepository = _fixture.GetRepositoryMock();
        var request = new ListCategoriesRequest(
            page: 2,
            perPage: 15,
            search: "search-example",
            sort: "name",
            dir: SearchOrder.Asc
            );
        var repositoryResponse = new ResponseSearch<Category>(
                    currentPage: request.Page,
                    perPage: request.PerPage,
                    Items: (IReadOnlyList<Category>)aCategoryList,
                    Total: 70);

        aRepository.Setup(category => category.Search(
            It.Is<SearchRequest>(
                    searchRequest.Page == request.Page &&
                    searchRequest.PerPage == request.PerPage &&
                    searchRequest.Search == request.Search &&
                    searchRequest.OrderBy == request.Sort &&
                    searchRequest.Order == request.Dir
                ),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(repositoryResponse);

        var useCase = new ListCategories(aRepository.Object);

        //when
        var response = await useCase.Handle(request, CancellationToken.None);

        //then
        response.Should().NotBeNull();
        response.Page.Should().Be(repositoryResponse.currentPage);
        response.PerPage.Should().Be(repositoryResponse.perPage);
        response.Total.Should().Be(repositoryResponse.Total);
        response.Items.Should().HaveCount(repositoryResponse.Items.Count);
        response.Items.Foreach(item =>
        {
            var aCategory = repositoryResponse.Items[0].Find(a => a.Id == item.Id);
            item.Should().NotBeNull();
            item.Name.Should().Be(aCategory.Name);
            item.Description.Should().Be(aCategory.Description);
            item.IsActive.Should().Be(aCategory.IsActive);
            item.CreatedAt.Should().Be(aCategory.CreatedAt);
        });

        aRepository.Verify(category => category.Search(
            It.Is<SearchRequest>(
                    searchRequest.Page == request.Page &&
                    searchRequest.PerPage == request.PerPage &&
                    searchRequest.Search == request.Search &&
                    searchRequest.OrderBy == request.Sort &&
                    searchRequest.Order == request.Dir
                ),
            It.IsAny<CancellationToken>()
            )
    }
}
