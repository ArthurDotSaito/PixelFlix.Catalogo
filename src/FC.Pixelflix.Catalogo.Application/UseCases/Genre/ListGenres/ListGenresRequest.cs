using FC.Pixelflix.Catalogo.Application.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.ListGenres;
using FC.Pixelflix.Catalogo.Domain.SeedWork.SearchableRepository;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;

public class ListGenresRequest : PaginatedListRequest, IRequest<ListGenresResponse>
{
    public ListGenresRequest(int page, int perPage, string? search, string? sort, SearchOrder dir) : base(page, perPage, search, sort, dir)
    {
    }
}