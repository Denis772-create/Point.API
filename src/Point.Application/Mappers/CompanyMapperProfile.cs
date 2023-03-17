namespace Point.Application.Mappers;

public class CompanyMapperProfile : Profile
{
    public CompanyMapperProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ReverseMap();

        CreateMap<Shop, ShopDto>()
            .ReverseMap();
    }
}