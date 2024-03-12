using MediatR;

namespace FC.Pixelflix.Catalogo.Application.UseCases.Category.DeleteCategory;
public class DeleteCategoryRequest : IRequest<Unit>
{
    public Guid Id { get; set; }

    public DeleteCategoryRequest(Guid id) {
        Id = id;
    }
}
