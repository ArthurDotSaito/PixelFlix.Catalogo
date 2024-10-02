using FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using FC.Pixelflix.Catalogo.Domain.Repository;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.ListGenres;

public class ListGenres : IListGenres
{
    private readonly IGenreRepository _genreRepository;

    public ListGenres(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<ListGenresResponse> Handle(ListGenresRequest request, CancellationToken cancellationToken)
    {
        var searchRequest = new SearchRepositoryRequest(request.Page, request.PerPage, request.Search, request.Sort, request.Dir);
        var searchResponse = await _genreRepository.Search(searchRequest, cancellationToken);

        return new ListGenresResponse(searchResponse.CurrentPage, searchResponse.PerPage, searchResponse.Total,
            searchResponse.Items.Select(GenreModelResponse.FromGenre).ToList());
    }
}