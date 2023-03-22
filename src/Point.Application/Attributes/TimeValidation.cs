namespace Point.Application.Attributes
{
    public class TimeValidation:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var time = value as TimeOnly?;

            if (time != null && time.HasValue) 
            {
                return time.Value != default
                    ? ValidationResult.Success
                    : new ValidationResult("Invalid time value");
            }

            return ValidationResult.Success;
        }
    }
}
