using Xunit;

namespace FC.Pixelflix.Catalogo.IntegrationTests.Infra.Data.EF.UnitOfWork;

[Collection(nameof(UnitOfWorkTestFixtureCollection))]
public class UnitOfWorkTest
{
    private readonly UnitOfWorkTestFixture _fixture;
    
    public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
    {
        _fixture = fixture;
    }
}