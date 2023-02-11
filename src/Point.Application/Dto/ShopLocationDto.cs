namespace Point.Application.Dto;

public class ShopLocationDto
{
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
    public string BuildingNumber { get; set; }
    public int EntranceNumber { get; set; }
    public int FloorNumber { get; set; }

    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }

    public static ShopLocationDto MapToShopLocationDto(ShopLocation shop)
        => new ShopLocationDto
        {
            BuildingNumber = shop.BuildingNumber,
            City = shop.City,
            Country = shop.Country,
            ZipCode = shop.ZipCode,
            EntranceNumber = shop.EntranceNumber,
            FloorNumber = shop.FloorNumber,
            Latitude = shop.Location.Coordinate.Y,
            Longitude = shop.Location.Coordinate.X,
            Street = shop.Street
        };
}