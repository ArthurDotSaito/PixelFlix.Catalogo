using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.CreateGenre.Dto;
using FC.Pixelflix.Catalogo.Domain.Repository;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.CreateGenre;

public class CreateGenre : ICreateGenre
{
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public Task<GenreModelResponse> Handle(CreateGenreRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}