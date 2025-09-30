
using System.ComponentModel.DataAnnotations;

namespace Domain.Attributes
{
    public class PositiveDecimalAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is decimal d && d > 0)
                return ValidationResult.Success;

            return new ValidationResult("Input must be greater than zero.");
        }
    }
}
