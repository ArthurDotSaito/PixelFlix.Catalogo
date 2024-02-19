
using FC.Pixelflix.Catalogo.Domain.Exceptions;

namespace FC.Pixelflix.Catalogo.Domain.Validation;
public class DomainValidation
{
    public static void NotNullValidation(object? target, string fieldName)
    {
        if(target == null)
        {
            throw new EntityValidationException($"{fieldName} should not be null");
        }
    }
    public static void NotNullOrEmptyValidation(string? target, string fieldName)
    {
        if (String.IsNullOrWhiteSpace(target))
        {
            throw new EntityValidationException($"{fieldName} should not be null or empty");
        }
    }
}
