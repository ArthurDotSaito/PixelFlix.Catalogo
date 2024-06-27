using FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
using FC.Pixelflix.Catalogo.e2e.API.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.e2e.API.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryFixtureCollection))]
public class UpdateCategoryFixtureCollection : ICollectionFixture<UpdateCategoryApiTestFixture>
{
}

public class UpdateCategoryApiTestFixture : CategoryBaseFixture
{
    public UpdateCategoryRequest GetAValidUpdateCategoryRequest(Guid? guid = null)
    {
        var aName = GetValidCategoryName();
        var aDescription = GetValidCategoryDescription();
        var isActive = GetRandomIsActive();
        
        return new UpdateCategoryRequest(guid ?? Guid.NewGuid(),aName, aDescription, isActive);
    }
}