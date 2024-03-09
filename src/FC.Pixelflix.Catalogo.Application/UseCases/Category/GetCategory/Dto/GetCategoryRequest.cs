using FC.Pixelflix.Catalogo.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory.Dto;
public class GetCategoryRequest : IRequest<CategoryModelResponse>
{
    public Guid Id { get; set; }
    public GetCategoryRequest(Guid id) => Id = id;

}
