﻿using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre.Dto;
using FC.Pixelflix.Catalogo.Domain.Repository;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre;

public class UpdateGenre : IUpdateGenre
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGenre(ICategoryRepository categoryRepository, IGenreRepository genreRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
    }

    public Task<GenreModelResponse> Handle(UpdateGenreRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}