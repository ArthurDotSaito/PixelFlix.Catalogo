using FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.Common;
using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Application.UseCases.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestFixtureCollection))]
public class UpdateCategoryTestFixtureCollection: ICollectionFixture<UpdateCategoryTestFixture> { }

public class UpdateCategoryTestFixture : CategoryUseCaseBaseFixture
{
    
}