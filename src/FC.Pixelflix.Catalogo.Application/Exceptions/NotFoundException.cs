namespace FC.Pixelflix.Catalogo.Application.Exceptions;
public class NotFoundException : ApplicationException
{
    public NotFoundException(string? message) : base(message)
    {
    }
}
