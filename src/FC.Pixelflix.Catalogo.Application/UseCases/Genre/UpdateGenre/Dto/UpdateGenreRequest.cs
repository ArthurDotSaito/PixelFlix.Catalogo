using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre.Dto;

public class UpdateGenreRequest: IRequest<GenreModelResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool? IsActive { get; set; } = null;

    public UpdateGenreRequest(Guid id, string name, bool? isActive)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
    }
}