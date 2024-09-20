using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.GetGenre.Dto;
using FC.Pixelflix.Catalogo.Domain.Repository;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.GetGenre;

public class GetGenre : IGetGenre
{
    private readonly IGenreRepository _genreRepository;
    
    public GetGenre(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }
    
    public async Task<GenreModelResponse> Handle(GetGenreRequest request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.Get(request.Id, cancellationToken);
        return GenreModelResponse.FromGenre(genre);
    }
}