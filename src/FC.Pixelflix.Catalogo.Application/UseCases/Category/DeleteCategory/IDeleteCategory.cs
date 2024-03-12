using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.DeleteCategory;
public interface IDeleteCategory : IRequestHandler<DeleteCategoryRequest, Unit>{}
