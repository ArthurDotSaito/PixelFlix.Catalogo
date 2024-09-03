using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.CreateGenre.Dto;

public class CreateGenreRequest : IRequest<GenreModelResponse>
{
    public string Name { get; set; }
    public bool IsActive { get; set; }

    public CreateGenreRequest(string name, bool isActive)
    {
        Name = name;
        IsActive = isActive;
    }
}