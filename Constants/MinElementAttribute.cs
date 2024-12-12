using System.Collections;
using System.ComponentModel.DataAnnotations;

public class MinElementsAttribute : ValidationAttribute
{
    private readonly int _minElements;

    public MinElementsAttribute(int minElements)
    {
        _minElements = minElements;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IList list && list.Count >= _minElements)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage ?? $"The list must contain at least {_minElements} element(s).");
    }
}