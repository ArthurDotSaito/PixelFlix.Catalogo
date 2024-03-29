﻿using UseCase = FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
using CategoryClass = FC.Pixelflix.Catalogo.Domain.Entities;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using FluentAssertions;
using Moq;
using Xunit;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Category.ListCategories;

[Collection(nameof(ListCategoriesTestCollection))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;

    public ListCategoriesTest(ListCategoriesTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GivenAValidRequest_WhenCallsListCategories_ShouldReturnOk))]
    [Trait("Application", "ListCategories - UseCases")]
    public async Task GivenAValidRequest_WhenCallsListCategories_ShouldReturnOk()
    {
        //given
        var aCategoryList = _fixture.GetValidCategoryList();
        var aRepository = _fixture.GetMockRepository();
        var request = _fixture.GetValidRequest();

        var repositoryResponse = new SearchRepositoryResponse<CategoryClass.Category>(
                    currentPage: request.Page,
                    perPage: request.PerPage,
                    items: (IReadOnlyList<CategoryClass.Category>)aCategoryList,
                    total: new Random().Next(50, 200)
        );

        aRepository.Setup(category => category.Search(
            It.Is<SearchRepositoryRequest>(searchRequest =>
                    searchRequest.Page == request.Page &&
                    searchRequest.PerPage == request.PerPage &&
                    searchRequest.Search == request.Search &&
                    searchRequest.OrderBy == request.Sort &&
                    searchRequest.Order == request.Dir
                ),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(repositoryResponse);

        var useCase = new UseCase.ListCategories(aRepository.Object);

        //when
        var response = await useCase.Handle(request, CancellationToken.None);

        //then
        response.Should().NotBeNull();
        response.Page.Should().Be(repositoryResponse.CurrentPage);
        response.PerPage.Should().Be(repositoryResponse.PerPage);
        response.Total.Should().Be(repositoryResponse.Total);
        response.Items.Should().HaveCount(repositoryResponse.Items.Count);
        ((List<CategoryModelResponse>)response.Items).ForEach(item =>
        {
            var aCategory = repositoryResponse.Items.FirstOrDefault(a => a.Id == item.Id);
            item.Should().NotBeNull();
            item.Name.Should().Be(aCategory.Name);
            item.Description.Should().Be(aCategory.Description);
            item.IsActive.Should().Be(aCategory.IsActive);
            item.CreatedAt.Should().Be(aCategory.CreatedAt);
        });

        aRepository.Verify(category => category.Search(
            It.Is<SearchRepositoryRequest>(searchRequest =>
                    searchRequest.Page == request.Page &&
                    searchRequest.PerPage == request.PerPage &&
                    searchRequest.Search == request.Search &&
                    searchRequest.OrderBy == request.Sort &&
                    searchRequest.Order == request.Dir
                ),
            It.IsAny<CancellationToken>()
            ));
    }

    [Theory(DisplayName = nameof(GivenAValidDefaultRequest_WhenCallsListCategories_ShouldReturnOk))]
    [Trait("Application", "ListCategories - UseCases")]
    [MemberData(
        nameof(ListCategoryDataGenerator.GetRequestWithDefaultParams),
        parameters: 12,
        MemberType = typeof(ListCategoryDataGenerator)
    )]
    public async Task GivenAValidDefaultRequest_WhenCallsListCategories_ShouldReturnOk(ListCategoriesRequest request)
    {
        //given
        var aCategoryList = _fixture.GetValidCategoryList();
        var aRepository = _fixture.GetMockRepository();

        var repositoryResponse = new SearchRepositoryResponse<CategoryClass.Category>(
                    currentPage: request.Page,
                    perPage: request.PerPage,
                    items: (IReadOnlyList<CategoryClass.Category>)aCategoryList,
                    total: new Random().Next(50, 200)
        );

        aRepository.Setup(category => category.Search(
            It.Is<SearchRepositoryRequest>(searchRequest =>
                    searchRequest.Page == request.Page &&
                    searchRequest.PerPage == request.PerPage &&
                    searchRequest.Search == request.Search &&
                    searchRequest.OrderBy == request.Sort &&
                    searchRequest.Order == request.Dir
                ),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(repositoryResponse);

        var useCase = new UseCase.ListCategories(aRepository.Object);

        //when
        var response = await useCase.Handle(request, CancellationToken.None);

        //then
        response.Should().NotBeNull();
        response.Page.Should().Be(repositoryResponse.CurrentPage);
        response.PerPage.Should().Be(repositoryResponse.PerPage);
        response.Total.Should().Be(repositoryResponse.Total);
        response.Items.Should().HaveCount(repositoryResponse.Items.Count);
        ((List<CategoryModelResponse>)response.Items).ForEach(item =>
        {
            var aCategory = repositoryResponse.Items.FirstOrDefault(a => a.Id == item.Id);
            item.Should().NotBeNull();
            item.Name.Should().Be(aCategory.Name);
            item.Description.Should().Be(aCategory.Description);
            item.IsActive.Should().Be(aCategory.IsActive);
            item.CreatedAt.Should().Be(aCategory.CreatedAt);
        });

        aRepository.Verify(category => category.Search(
            It.Is<SearchRepositoryRequest>(searchRequest =>
                    searchRequest.Page == request.Page &&
                    searchRequest.PerPage == request.PerPage &&
                    searchRequest.Search == request.Search &&
                    searchRequest.OrderBy == request.Sort &&
                    searchRequest.Order == request.Dir
                ),
            It.IsAny<CancellationToken>()
            ));
    }

    [Fact(DisplayName = nameof(GivenAValidRequest_WhenReturnEmpty_ShouldNeOk))]
    [Trait("Application", "ListCategories - UseCases")]
    public async Task GivenAValidRequest_WhenReturnEmpty_ShouldNeOk()
    {
        //given
        var aRepository = _fixture.GetMockRepository();
        var request = _fixture.GetValidRequest();

        var repositoryResponse = new SearchRepositoryResponse<CategoryClass.Category>(
                    currentPage: request.Page,
                    perPage: request.PerPage,
                    items: new List<CategoryClass.Category>().AsReadOnly(),
                    total: 0
        );

        aRepository.Setup(category => category.Search(
            It.Is<SearchRepositoryRequest>(searchRequest =>
                    searchRequest.Page == request.Page &&
                    searchRequest.PerPage == request.PerPage &&
                    searchRequest.Search == request.Search &&
                    searchRequest.OrderBy == request.Sort &&
                    searchRequest.Order == request.Dir
                ),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(repositoryResponse);

        var useCase = new UseCase.ListCategories(aRepository.Object);

        //when
        var response = await useCase.Handle(request, CancellationToken.None);

        //then
        response.Should().NotBeNull();
        response.Page.Should().Be(repositoryResponse.CurrentPage);
        response.PerPage.Should().Be(repositoryResponse.PerPage);
        response.Total.Should().Be(0);
        response.Items.Should().HaveCount(0);

        aRepository.Verify(category => category.Search(
            It.Is<SearchRepositoryRequest>(searchRequest =>
                    searchRequest.Page == request.Page &&
                    searchRequest.PerPage == request.PerPage &&
                    searchRequest.Search == request.Search &&
                    searchRequest.OrderBy == request.Sort &&
                    searchRequest.Order == request.Dir
                ),
            It.IsAny<CancellationToken>()
            ));
    }


}
