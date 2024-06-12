using FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
using FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixtureCollection))]
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> { };

public class CreateCategoryTestFixture : CategoryUseCaseBaseFixture
{
    public CreateCategoryRequest GetValidRequest()
    {
        var category = GetValidCategory();
        return new CreateCategoryRequest(category.Name, category.Description, category.IsActive);
    }  
    
}