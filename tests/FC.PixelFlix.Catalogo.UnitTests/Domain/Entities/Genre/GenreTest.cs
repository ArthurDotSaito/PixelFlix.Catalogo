using FluentAssertions;
using Xunit;
using GenreDomain = FC.Pixelflix.Catalogo.Domain.Entities.Genre;

namespace FC.PixelFlix.Catalogo.UnitTests.Domain.Entities.Genre;

[Collection(nameof(GenreTestFixture))]
public class GenreTest
{
   [Fact(DisplayName = nameof(GivenAGenreNewInstance_WhenEverythingIsValid_ShouldBeInstantiateAGenre))]
   [Trait("Domain", "Genre - Aggregates")]
   public void GivenAGenreNewInstance_WhenEverythingIsValid_ShouldBeInstantiateAGenre()
   {
      var expectedName = "Horror";
      var dateTimeBefore = DateTime.Now;
      var dateTimeAfter = DateTime.Now.AddSeconds(1);
      
      var genre = new GenreDomain(expectedName);
      
      genre.Should().NotBeNull();
      genre.Name.Should().Be(expectedName);
      genre.IsActive.Should().BeTrue();
      genre.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
      genre.CreatedAt.Should().BeAfter(dateTimeBefore);
      genre.CreatedAt.Should().BeBefore(dateTimeAfter);
   }
   
}