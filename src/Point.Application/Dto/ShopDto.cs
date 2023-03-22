using Point.API.Attributes;
using Point.Application.Attributes;

namespace Point.Application.Dto;

public class ShopDto
{
    [PhoneValidation]
    public string Phone { get; set; }

    [TimeValidation]
    public TimeOnly? OpeningTime { get; set; }

    [TimeValidation]
    public TimeOnly? ClosingTime { get; set; }

    [ContentValidation(30)]
    public string Street { get; set; }

    [ContentValidation(30)]
    public string City { get; set; }

    [ContentValidation(30)]
    public string Country { get; set; }

    [ContentValidation(8)]
    public string ZipCode { get; set; }

    [ContentValidation(3)]
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