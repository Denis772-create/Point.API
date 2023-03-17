namespace Point.Domain.Entities.CompanyAggregate;

public class ShopLocation : ValueObject
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }
    public string ZipCode { get; private set; }
    public string BuildingNumber { get; private set; }
    public int EntranceNumber { get; private set; }
    public int FloorNumber { get; private set; }
    public Topology.Point Location { get; private set; }

    public ShopLocation(string buildingNumber, int entranceNumber,
        int floorNumber, string street, string city, string country,
        string zipCode, NetTopologySuite.Geometries.Point location)
    {
        BuildingNumber = buildingNumber;
        EntranceNumber = entranceNumber;
        FloorNumber = floorNumber;
        Street = street;
        City = city;
        Country = country;
        ZipCode = zipCode;
        Location = location;
    }

    public ShopLocation() { }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Street;
        yield return Country;
        yield return ZipCode;
        yield return BuildingNumber;
        yield return EntranceNumber;
        yield return FloorNumber;
        yield return City;
        yield return Location;
    }
}