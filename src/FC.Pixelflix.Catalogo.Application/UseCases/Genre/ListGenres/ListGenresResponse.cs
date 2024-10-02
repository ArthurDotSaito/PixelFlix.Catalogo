using FC.Pixelflix.Catalogo.Application.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.ListGenres;

public class ListGenresResponse: PaginatedListResponse<GenreModelResponse>, IRequest<ListGenresResponse>
{
    public ListGenresResponse(int page, int perPage, int total, IReadOnlyList<GenreModelResponse> items) : base(page, perPage, total, items)
    {
    }
}