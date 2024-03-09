using FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory.Dto;
using FluentValidation;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.GetCategory;
public class GetCategoryRequestValidation : AbstractValidator<GetCategoryRequest>
{
    public GetCategoryRequestValidation()
    {
        RuleFor(category => category.Id).NotEmpty();
    }
}
