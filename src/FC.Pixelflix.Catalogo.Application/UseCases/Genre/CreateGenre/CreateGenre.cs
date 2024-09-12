using FC.Pixelflix.Catalogo.Application.Exceptions;
using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.CreateGenre.Dto;
using FC.Pixelflix.Catalogo.Domain.Repository;
using DomainGenre = FC.Pixelflix.Catalogo.Domain.Entities.Genre;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.CreateGenre;

public class CreateGenre : ICreateGenre
{
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGenre(IGenreRepository genreRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<GenreModelResponse> Handle(CreateGenreRequest request, CancellationToken cancellationToken)
    {
        var genre = new DomainGenre(request.Name, request.IsActive);
        if (request.Categories is not null)
        {
            await ValidateCateogriesIds(request, cancellationToken);
            request.Categories.ForEach(genre.AddCategory);
        }

        await _genreRepository.Insert(genre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return GenreModelResponse.FromGenre(genre);
    }

    private async Task ValidateCateogriesIds(CreateGenreRequest request, CancellationToken cancellationToken)
    {
        var categoriesIds = await _categoryRepository.GetIdsListByIds(request.Categories!, cancellationToken);

        if (categoriesIds.Count < request.Categories!.Count)
        {
            var notFoundCategories = request.Categories.FindAll(e => !categoriesIds.Contains(e));
            throw new RelatedAggregateException($"Related categories not found: {string.Join(", ", notFoundCategories)}");
        }
    }

}