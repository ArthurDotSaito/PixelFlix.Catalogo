using FC.PixelFlix.Catalogo.UnitTests.Application.Category.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryFixtureCollection : ICollectionFixture<GetCategoryTestFixture> { }
public class GetCategoryTestFixture : CategoryUseCasesBaseFixture { }
