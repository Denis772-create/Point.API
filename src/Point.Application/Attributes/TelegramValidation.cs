using System.Text.RegularExpressions;

namespace Point.Application.Attributes
{
    public class TelegramValidation: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var telegramLink = value as string;
            if (telegramLink != null) 
            {
                string pattern = @"^(https?:\/\/)?(www\.)?t\.me\/[a-zA-Z0-9_]+$";
                return Regex.IsMatch(telegramLink, pattern)
                    ? ValidationResult.Success
                    : new ValidationResult("Invalid telegram link");
            }

            return ValidationResult.Success;
        }
    }
}
