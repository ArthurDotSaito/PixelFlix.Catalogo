using FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory.Dto;
using FC.Pixelflix.Catalogo.e2e.API.Common;
using Xunit;


namespace FC.Pixelflix.Catalogo.e2e.API.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryApiTestFixtureCollection))]
public class CreateCategoryApiTestFixtureCollection : ICollectionFixture<CreateCategoryApiTestFixture>{} 

public class CreateCategoryApiTestFixture : CategoryBaseFixture
{
    public CreateCategoryRequest GetAValidCreateCategoryRequest()
    {
        var aCategoryName = GetValidCategoryName();
        var aCategoryDescription = GetValidCategoryDescription();
        var aIsActive = GetRandomIsActive();
        
        return new CreateCategoryRequest(aCategoryName, aCategoryDescription, aIsActive);
    }
}