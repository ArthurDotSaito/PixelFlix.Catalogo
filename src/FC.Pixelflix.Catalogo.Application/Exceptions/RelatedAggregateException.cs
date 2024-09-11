namespace FC.Pixelflix.Catalogo.Application.Exceptions;

public class RelatedAggregateException : ApplicationException
{
    public RelatedAggregateException(string? message) : base(message) { }
    
}