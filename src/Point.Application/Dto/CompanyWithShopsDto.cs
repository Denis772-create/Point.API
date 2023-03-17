namespace Point.Application.Dto;

public class CompanyWithShopsDto : CompanyDto
{
    public List<ShopDto> Shops { get; set; }

    public new static CompanyWithShopsDto MapFromCompany(Company company)
        => new()
        {
            Instagram = company.Instagram,
            Name = company.Name,
            SupportNumber = company.SupportNumber,
            Telegram = company.Telegram,
            Shops = company.Shops.Select(x => new ShopDto
            {
                City = x.ShopLocation.City,
                Street = x.ShopLocation.Street,
                Country = x.ShopLocation.Country,
                BuildingNumber = x.ShopLocation.BuildingNumber,
                EntranceNumber = x.ShopLocation.EntranceNumber,
                Phone = x.Phone,
                FloorNumber = x.ShopLocation.FloorNumber,
                ZipCode = x.ShopLocation.ZipCode,
                Latitude = x.ShopLocation.Location.Coordinate.Y,
                Longitude = x.ShopLocation.Location.Coordinate.X,
                ClosingTime = x.ClosingTime,
                OpeningTime = x.OpeningTime
            }).ToList()
        };
}