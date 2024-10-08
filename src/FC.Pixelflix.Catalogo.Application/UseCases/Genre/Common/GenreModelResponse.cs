﻿namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using GenreDomain = FC.Pixelflix.Catalogo.Domain.Entities.Genre;
public class GenreModelResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public IReadOnlyList<Guid> Categories { get; set; }

    public GenreModelResponse(Guid id, string name, bool isActive, DateTime createdAt, IReadOnlyList<Guid> categories)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        Categories = categories;
    }
    
    public static GenreModelResponse FromGenre(GenreDomain genre)
    {
        return new GenreModelResponse(genre.Id, genre.Name,genre.IsActive, genre.CreatedAt, genre.Categories);
    }
}