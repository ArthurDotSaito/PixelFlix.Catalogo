using FC.Pixelflix.Catalogo.Application.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.ListGenres;

public class ListGenresResponse: PaginatedListResponse<GenreModelResponse>, IRequest<ListGenresResponse>
{
    public ListGenresResponse(int page, int perPage, int total, IReadOnlyList<GenreModelResponse> items) : base(page, perPage, total, items)
    {
    }
    
    public static ListGenresResponse FromSearchResponse(SearchRepositoryResponse<Domain.Entities.Genre> searchResponse)
    {
        return new ListGenresResponse(searchResponse.CurrentPage, searchResponse.PerPage, searchResponse.Total, 
            searchResponse.Items.Select(GenreModelResponse.FromGenre).ToList());
    }
}