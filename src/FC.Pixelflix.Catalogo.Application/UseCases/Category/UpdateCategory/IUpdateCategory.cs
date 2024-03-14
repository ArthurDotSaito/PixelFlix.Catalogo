
using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
public interface IUpdateCategory: IRequestHandler<UpdateCategoryRequest, CategoryModelResponse>{}
