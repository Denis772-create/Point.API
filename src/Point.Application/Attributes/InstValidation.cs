using System.Text.RegularExpressions;

namespace Point.Application.Attributes
{
    public class InstValidation: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var instagramLink = value as string;
            if (instagramLink != null) 
            {
                string pattern = @"^(https?:\/\/)?(www\.)?instagram\.com\/[a-zA-Z0-9_\.]+$";
                return Regex.IsMatch(instagramLink, pattern)
                    ? ValidationResult.Success
                    : new ValidationResult("Invalid instagram link");
            }

            return ValidationResult.Success;
        }
    }
}
