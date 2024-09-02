using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.CreateGenre.Dto;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.CreateGenre;

public interface ICreateGenre: IRequestHandler<CreateGenreRequest, GenreModelResponse>
{
    
}