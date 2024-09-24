using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.DeleteGenre.Dto;

public class DeleteGenreRequest : IRequest
{
    public Guid Id { get; set; }

    public DeleteGenreRequest(Guid id)
    {
        Id = id;
    }
}