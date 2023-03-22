using System.Text.RegularExpressions;

namespace Point.API.Attributes
{
    public class PhoneValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var phoneNumber = value as string;
            if (phoneNumber != null)
            {
                var regex = new Regex(@"^375(33|29|44|25)\d{7}$");

                return regex.IsMatch(phoneNumber)
                    ? ValidationResult.Success
                    : new ValidationResult("Invalid phone number");
            }

            return new ValidationResult("Phone number is requared");
        }
    }
}
