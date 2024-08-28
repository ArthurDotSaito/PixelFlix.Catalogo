﻿using FluentAssertions;
using Xunit;
using GenreDomain = FC.Pixelflix.Catalogo.Domain.Entities.Genre;

namespace FC.PixelFlix.Catalogo.UnitTests.Domain.Entities.Genre;

[Collection(nameof(GenreTestFixture))]
public class GenreTest
{

   private readonly GenreTestFixture _fixture;

   public GenreTest(GenreTestFixture fixture)
   {
      _fixture = fixture;
   }
   
   [Fact(DisplayName = nameof(GivenAGenreNewInstance_WhenEverythingIsValid_ShouldBeInstantiateAGenre))]
   [Trait("Domain", "Genre - Aggregates")]
   public void GivenAGenreNewInstance_WhenEverythingIsValid_ShouldBeInstantiateAGenre()
   {
      var expectedName = _fixture.GetValidName();
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
   
   [Theory(DisplayName = nameof(GivenAGenreNewInstance_WhenIsActiveTrue_ShouldBeInstantiateAGenre))]
   [Trait("Domain", "Genre - Aggregates")]
   [InlineData(true)]
   [InlineData(false)]
   public void GivenAGenreNewInstance_WhenIsActiveTrue_ShouldBeInstantiateAGenre(bool isActive)
   {
      var expectedName = _fixture.GetValidName();
      var dateTimeBefore = DateTime.Now;
      var dateTimeAfter = DateTime.Now.AddSeconds(1);
      
      var genre = new GenreDomain(expectedName, isActive);
      
      genre.Should().NotBeNull();
      genre.Name.Should().Be(expectedName);
      genre.IsActive.Should().Be(isActive);
      genre.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
      genre.CreatedAt.Should().BeAfter(dateTimeBefore);
      genre.CreatedAt.Should().BeBefore(dateTimeAfter);
   }
   
   [Theory(DisplayName = nameof(GivenAGenreNewInstance_WhenIsActiveTrue_ShouldBeInstantiateAGenre))]
   [Trait("Domain", "Genre - Aggregates")]
   [InlineData(true)]
   [InlineData(false)]
   public void GivenAGenre_WhenCallActivate_ShouldIsActiveTrue(bool isActive)
   {
      var expectedName = _fixture.GetValidName();
      var genre = new GenreDomain(expectedName, isActive);

      genre.Activate();

      genre.Should().NotBeNull();
      genre.Name.Should().Be(expectedName);
      genre.IsActive.Should().Be(isActive);
      genre.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
   }
   
}