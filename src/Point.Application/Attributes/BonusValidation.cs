namespace Point.Application.Attributes
{
    public class BonusValidation: ValidationAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public BonusValidation()
        {
            _min = 5;
            _max = 12;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var countBonuses = value as int?;
            if (countBonuses != null && countBonuses.HasValue)
            {
                return countBonuses.Value >= _min && countBonuses <= _max
                   ? ValidationResult.Success
                   : new ValidationResult($"Count bonuses from {_min} to {_max}");
            }

            return new ValidationResult("Count of bonuses is requared");
        }
    }
}
