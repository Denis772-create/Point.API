namespace Point.Application.Dto;

public class ShopDto
{
    public string? Phone { get; set; }
    public TimeOnly? OpeningTime { get; set; }
    public TimeOnly? ClosingTime { get; set; }
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

    public static ShopLocation MapToShopLocation(ShopDto shopDto, GeometryFactory geometryFactory)
        => new ShopLocation(shopDto.BuildingNumber,
            shopDto.EntranceNumber,
            shopDto.FloorNumber,
            shopDto.Street,
            shopDto.City,
            shopDto.Country,
            shopDto.ZipCode,
            geometryFactory.CreatePoint(
                new Coordinate(shopDto.Longitude, shopDto.Latitude)));
}