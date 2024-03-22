using FC.PixelFlix.Catalogo.UnitTests.Application.Category.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryFixtureCollection))]
public class DeleteCategoryFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture> { };

public class DeleteCategoryTestFixture : CategoryUseCasesBaseFixture
{ }
