
using FluentValidation;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.UpdateCategory;
public class UpdateCategoryRequestValidation : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidation()
    {
        RuleFor(category => category.Id).NotEmpty();
    }
}
