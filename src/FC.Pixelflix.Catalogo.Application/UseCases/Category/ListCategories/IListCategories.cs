using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.ListCategories;
public interface IListCategories : IRequestHandler<ListCategoriesRequest, ListCategoriesResponse>{}
