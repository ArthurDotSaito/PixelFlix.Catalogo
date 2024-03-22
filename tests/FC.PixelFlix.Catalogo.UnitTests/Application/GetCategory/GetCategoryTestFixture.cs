using FC.PixelFlix.Catalogo.UnitTests.Application.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryFixtureCollection : ICollectionFixture<GetCategoryTestFixture> { }
public class GetCategoryTestFixture : CategoryUseCasesBaseFixture{}
