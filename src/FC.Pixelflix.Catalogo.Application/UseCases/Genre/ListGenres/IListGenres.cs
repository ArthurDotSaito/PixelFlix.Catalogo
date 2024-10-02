using FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.ListGenres;

public interface IListGenres : IRequestHandler<ListGenresRequest, ListGenresResponse>
{
    
}