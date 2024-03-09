using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory.Dto;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory;
public interface IGetCategory : IRequestHandler<GetCategoryRequest, CategoryModelResponse>
{   

}
