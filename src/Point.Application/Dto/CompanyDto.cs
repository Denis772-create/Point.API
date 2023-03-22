using Point.API.Attributes;
using Point.Application.Attributes;

namespace Point.Application.Dto;

public class CompanyDto
{
    [ContentValidation(50)]
    public string Name { get; set; }

    [InstValidation]
    public string? Instagram { get; set; }

    [TelegramValidation]
    public string? Telegram { get; set; }

    [PhoneValidation]
    public string SupportNumber { get; set; }
}