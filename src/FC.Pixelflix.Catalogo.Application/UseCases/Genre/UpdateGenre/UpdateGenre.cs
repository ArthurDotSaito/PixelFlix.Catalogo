using FC.Pixelflix.Catalogo.Application.Interfaces;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre.Dto;
using FC.Pixelflix.Catalogo.Domain.Repository;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Genre.UpdateGenre;

public class UpdateGenre : IUpdateGenre
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGenre(ICategoryRepository categoryRepository, IGenreRepository genreRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GenreModelResponse> Handle(UpdateGenreRequest request, CancellationToken cancellationToken)
    {
        var aGenre = await _genreRepository.Get(request.Id, cancellationToken);
        
        aGenre.Update(request.Name);

        if (request.IsActive != aGenre.IsActive && request.IsActive is not null)
        {
            if((bool)request.IsActive) aGenre.Activate();
            else
            {
                aGenre.Deactivate();
            }
        }

        if ((request.CategoryIds?.Count ?? 0) > 0)
        {
            aGenre.RemoveAllCategories();
            request.CategoryIds?.ForEach(categoryId => aGenre.AddCategory(categoryId));
        }
        
        await _genreRepository.Update(aGenre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
            
        return GenreModelResponse.FromGenre(aGenre);
    }
}