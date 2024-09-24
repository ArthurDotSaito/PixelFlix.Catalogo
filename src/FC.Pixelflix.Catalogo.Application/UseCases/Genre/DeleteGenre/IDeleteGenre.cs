using FC.Pixelflix.Catalogo.Application.UseCases.Genre.DeleteGenre.Dto;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.DeleteGenre;

public interface IDeleteGenre : IRequestHandler<DeleteGenreRequest>
{
    
}