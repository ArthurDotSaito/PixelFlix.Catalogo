﻿using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre.Dto;

public class UpdateGenreRequest: IRequest<GenreModelResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool? IsActive { get; set; } = null;
    public List<Guid>? CategoryIds { get; set; }

    public UpdateGenreRequest(Guid id, string name, bool? isActive = null, List<Guid> categoryIds = null)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CategoryIds = categoryIds;
    }
}