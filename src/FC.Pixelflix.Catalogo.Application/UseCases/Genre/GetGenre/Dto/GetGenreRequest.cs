using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.GetGenre.Dto;

public class GetGenreRequest: IRequest<GenreModelResponse>
{
    public Guid Id { get; set; }

    public GetGenreRequest(Guid id)
    {
        Id = id;
    }
}