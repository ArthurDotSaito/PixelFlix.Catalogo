using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre.Dto;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre;

public interface IUpdateGenre : IRequestHandler<UpdateGenreRequest, GenreModelResponse>
{
    
}