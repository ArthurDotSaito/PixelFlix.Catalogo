
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
            throw new EntityValidationException($"{fieldName} should not be empty or null");
        }
    }

    public static void MinLengthValidation(string target,int minLength, string fieldName)
    {
        if (target.Length < minLength)
        {
            throw new EntityValidationException($"{fieldName} should be at least {minLength} characters long");
        }
    }

    public static void MaxLengthValidation(string target, int maxLength, string fieldName)
    {
        if (target.Length > maxLength)
        {
            throw new EntityValidationException($"{fieldName} should be less than {maxLength} characters long");
        }
    }
}
