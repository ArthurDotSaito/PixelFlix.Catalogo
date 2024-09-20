using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.GetGenre.Dto;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.GetGenre;

public interface IGetGenre : IRequestHandler<GetGenreRequest, GenreModelResponse>
{
    
}