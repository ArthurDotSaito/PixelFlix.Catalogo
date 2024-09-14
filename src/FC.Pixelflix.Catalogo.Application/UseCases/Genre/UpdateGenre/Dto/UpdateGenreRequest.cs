using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre.Dto;

public class UpdateGenreRequest: IRequest<GenreModelResponse>
{
    public string Name { get; set; }
    public bool? IsActive { get; set; } = null;

    public UpdateGenreRequest(string name, bool? isActive)
    {
        Name = name;
        IsActive = isActive;
    }
}