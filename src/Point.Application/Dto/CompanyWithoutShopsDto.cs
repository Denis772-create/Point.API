namespace Point.Application.Dto;

public class CompanyWithoutShopsDto
{
    public string Name { get; set; }
    public string? Instagram { get; set; }
    public string? Telegram { get; set; }
    public string SupportNumber { get; set; }

    public static CompanyWithoutShopsDto MapFromCompany(Company company)
        => new CompanyWithoutShopsDto
        {
            Instagram = company.Instagram,
            Name = company.Name,
            SupportNumber = company.SupportNumber,
            Telegram = company.Telegram,
        };
}