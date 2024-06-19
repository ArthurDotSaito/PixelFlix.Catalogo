﻿using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.CreateCategory;

[Collection(nameof(CreateCategoryApiTestFixtureCollection))]
public class CreateCategoryApiTest
{
    private readonly CreateCategoryApiTestFixture _fixture;
    
    public CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "")]
    [Trait("E2E/API", "Category Endpoints")]
    public async Task GivenAValidCreateCategoryRequest_WhenCallsCreateCategory_ShouldCreateCategory()
    {
        //given
        var createCategoryRequest = _fixture.GetAValidCreateCategoryRequest();
        
        //when
        var response = await _fixture.Api.Post<CategoryModelResponse>("/categories", createCategoryRequest);
        
        //then
        response.Should().NotBeNull();
        response.Id.Should().NotBeEmpty();
        response.Name.Should().Be(createCategoryRequest.Name);
        response.Description.Should().Be(createCategoryRequest.Description);
        response.IsActive.Should().Be(createCategoryRequest.IsActive);
        response.CreatedAt.Should().NotBeSameDateAs(default(DateTime));

        var categoryInDatabase = _fixture.Persistence.GetById(response.Id);
        
        categoryInDatabase.Should().NotBeNull();
        categoryInDatabase.Id.Should().Be(response.Id);
        categoryInDatabase.Name.Should().Be(response.Name);
        categoryInDatabase.Description.Should().Be(response.Description);
        categoryInDatabase.IsActive.Should().Be(response.IsActive);
        categoryInDatabase.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }
    
}