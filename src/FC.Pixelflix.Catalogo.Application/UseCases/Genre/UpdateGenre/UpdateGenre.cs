using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre.Dto;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre;

public class UpdateGenre : IUpdateGenre
{
    public Task<GenreModelResponse> Handle(UpdateGenreRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}