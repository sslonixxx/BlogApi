namespace blog_api.Constants;

using System;
using System.ComponentModel.DataAnnotations;

public class MinAgeAttribute : ValidationAttribute
{
    private readonly int _minAge;

    public MinAgeAttribute(int minAge)
    {
        _minAge = minAge;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
            {
                age--;
            }

            if (age >= _minAge)
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult($"You must be at least 14 years old");
    }
}
