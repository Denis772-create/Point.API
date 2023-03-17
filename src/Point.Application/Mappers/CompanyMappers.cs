namespace Point.Application.Mappers;

public static class CompanyMappers
{
    private static IMapper Mapper { get; }

    static CompanyMappers()
    {
        Mapper = new MapperConfiguration(cnf =>
            cnf.AddProfile<CompanyMapperProfile>()).CreateMapper();
    }

    public static CompanyDto ToDto(this Company entity)
    {
        return entity is null ? null : Mapper.Map<CompanyDto>(entity);
    }

    public static Company ToEntity(this CompanyDto dto)
    {
        return dto is null ? null : Mapper.Map<Company>(dto);
    }

    public static ShopDto ToDto(this Shop entity)
    {
        return entity is null ? null : Mapper.Map<ShopDto>(entity);
    }

    public static Shop ToEntity(this ShopDto dto)
    {
        return dto is null ? null : Mapper.Map<Shop>(dto);
    }
}