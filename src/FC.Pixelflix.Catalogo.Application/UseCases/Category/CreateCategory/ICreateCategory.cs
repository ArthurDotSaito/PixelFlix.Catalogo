using FC.Pixelflix.Catalogo.Application.UseCases.Category.Dto;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory
{
    public Task<CreateCategoryOutput> Execute(CreateCategoryInput anInput, CancellationToken aCancellationToken);

}
