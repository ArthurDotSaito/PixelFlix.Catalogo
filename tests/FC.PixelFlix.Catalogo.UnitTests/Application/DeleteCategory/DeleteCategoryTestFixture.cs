using FC.PixelFlix.Catalogo.UnitTests.Application.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryFixtureCollection))]
public class DeleteCategoryFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture> { };

public class DeleteCategoryTestFixture : CategoryUseCasesBaseFixture
{}
