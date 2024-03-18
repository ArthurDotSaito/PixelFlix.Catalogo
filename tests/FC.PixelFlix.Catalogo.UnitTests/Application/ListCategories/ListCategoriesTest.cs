using FC.Pixelflix.Catalogo.Domain.Entities;
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

        aRepository.Setup(category => category.Search(
            It.Is<SearchRequest>(
                    searchRequest.Page == request.Page &&
                    searchRequest.PerPage == request.PerPage &&
                    searchRequest.Search == request.Search &&
                    searchRequest.OrderBy == request.Sort &&
                    searchRequest.Order == request.Dir
                ),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(new ResponseSearch<Category>(
                    currentPage:request.Page,
                    perPage: request.PerPage,
                    Items: (IReadOnlyList<Category>)aCategoryList,
                    Total: 70
            )
        );

        var useCase = new ListCategories(aRepository.Object);

        //when
        var response = await useCase.Handle(request, CancellationToken.None);

        //then
        response.Should().NotBeNull();
    }
}
