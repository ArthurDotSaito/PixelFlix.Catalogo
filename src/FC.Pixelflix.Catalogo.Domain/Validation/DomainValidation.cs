
using FC.Pixelflix.Catalogo.Domain.Exceptions;

namespace FC.Pixelflix.Catalogo.Domain.Validation;
public class DomainValidation
{
    public static void NotNullDomainValidation(object target, string fieldName)
    {
        if(target == null)
        {
            throw new EntityValidationException($"{fieldName} should not be null");
        }
    }
}
