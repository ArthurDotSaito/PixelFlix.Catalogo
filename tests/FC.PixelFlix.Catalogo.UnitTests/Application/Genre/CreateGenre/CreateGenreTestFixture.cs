using FC.Pixelflix.Catalogo.Application.UseCases.Genre.CreateGenre.Dto;
using FC.PixelFlix.Catalogo.UnitTests.Application.Genre.Common;
using Xunit;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.CreateGenre;

[CollectionDefinition(nameof(CreateGenreTestFixture))]
public class CreateGenreTestFixtureCollection(): ICollectionFixture<CreateGenreTestFixture> { }

public class CreateGenreTestFixture : GenreUseCasesBaseFixture
{
    public CreateGenreRequest GetValidInput()
    {
        var genreName = GetValidGenreName();
        var isActive = GetRandomIsActive();
        
        return new CreateGenreRequest(genreName, isActive);
    }
    
    public CreateGenreRequest GetValidInput(string? name)
    {
        var genreName = name;
        var isActive = GetRandomIsActive();
        
        return new CreateGenreRequest(genreName, isActive);
    }
    
    public CreateGenreRequest GetValidInputWithCategories()
    {
        var genreName = GetValidGenreName();
        var isActive = GetRandomIsActive();
        var categoriesIds = Enumerable.Range(0, (new Random()).Next(1, 10)).Select(_ => Guid.NewGuid()).ToList();
        
        return new CreateGenreRequest(genreName, isActive, categoriesIds);
    }
    

}