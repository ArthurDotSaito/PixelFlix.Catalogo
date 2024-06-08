using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using UnitOfWorkImplementation = FC.Pixelflix.Catalogo.Infra.Data.EF.UnitOfWork;


namespace FC.Pixelflix.Catalogo.IntegrationTests.Infra.Data.EF.UnitOfWork;

[Collection(nameof(UnitOfWorkTestFixtureCollection))]
public class UnitOfWorkTest
{
    private readonly UnitOfWorkTestFixture _fixture;
    
    public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Commit))]
    [Trait("Integration/Infra.Data ", "UnitOfWork - Persistence")]

    public async Task Commit()
    {
        //given
        var dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetValidCategoryList();
        await dbContext.AddRangeAsync(exampleCategoriesList);

        var unitOfWork = new UnitOfWorkImplementation(dbContext);
        
        //when
        await unitOfWork.Commit(CancellationToken.None);
        
        //then
        var assertDbContext = _fixture.CreateDbContext(true);
        var categoriesPersisted = assertDbContext.Categories.AsNoTracking().ToList();
        
        categoriesPersisted.Should().HaveCount(exampleCategoriesList.Count);
    }
}