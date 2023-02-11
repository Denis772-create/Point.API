namespace Point.Application.Interfaces;

public interface ICardTemplateService
{
    Task<Guid> CreateAsync(CardTemplateDto templateDto, CancellationToken ct);
    Task<Guid> CreateImageAsync(string imageName, CancellationToken ct);
    Task<Guid> UpdateAsync(CardTemplateDto templateDto, CancellationToken ct);
    Task<List<CardTemplateDto>> GetAllByCompanyIdAsync(Guid companyId, CancellationToken ct);
    Task<Guid> DeleteAsync(Guid id, CancellationToken ct);
}