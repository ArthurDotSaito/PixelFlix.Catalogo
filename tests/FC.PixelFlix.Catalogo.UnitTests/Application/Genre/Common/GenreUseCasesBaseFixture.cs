﻿using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Domain.Repository;
using FC.PixelFlix.Catalogo.UnitTests.Common;
using Moq;
using DomainGenre = FC.Pixelflix.Catalogo.Domain.Entities.Genre;

namespace FC.PixelFlix.Catalogo.UnitTests.Application.Genre.Common;

public class GenreUseCasesBaseFixture : BaseFixture
{
    public string GetValidGenreName()
    {
        return Faker.Commerce.Categories(1)[0];
    }
    
    public DomainGenre GetValidGenre()
    {
        var aName = GetValidGenreName();
        var isActive = GetRandomIsActive();
        return new DomainGenre(aName, isActive);
    }
    public DomainGenre GetValidGenre(bool? isActive = null)
    {
        var aName = GetValidGenreName();
        var active = isActive ?? GetRandomIsActive();
        return new DomainGenre(aName, active);
    }
    
    public DomainGenre GetValidGenreWithCategories(bool? isActive = null , List<Guid> categoryIds = null)
    {
        var aName = GetValidGenreName();
        var active = isActive ?? GetRandomIsActive();
        
        var genre = new DomainGenre(aName, active);
        categoryIds?.ForEach(categoryId => genre.AddCategory(categoryId));
        return genre;
    }
    
    public List<DomainGenre> GetValidGenreList(int count = 10)
    {
        return Enumerable.Range(1, count).Select(_ =>
        {
            var aName = GetValidGenreName();
            var isActive = GetRandomIsActive();
            var genre =  new DomainGenre(aName, isActive);
            GenerateRandomCategoryIds().ForEach(genre.AddCategory);
            return genre;
        }).ToList();
    }

    public List<Guid> GenerateRandomCategoryIds(int? count = null)
    {
        return Enumerable.Range(1, count ?? (new Random().Next(1, 10))).Select(_ => Guid.NewGuid()).ToList();
    }
    
    public Mock<IGenreRepository> GetGenreRepositoryMock()
    {
        return new Mock<IGenreRepository>();
    }
    
    public Mock<IUnitOfWork> GetUnitOfWorkMock()
    {
        return new Mock<IUnitOfWork>();
    }
    
    public Mock<ICategoryRepository> GetCategoryRepositoryMock()
    {
        return new Mock<ICategoryRepository>();
    }
}