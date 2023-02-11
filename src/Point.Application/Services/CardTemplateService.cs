namespace Point.Application.Services;

public class CardTemplateService : ICardTemplateService
{
    private readonly ICardTemplateRepository _cardTemplateRepository;

    public CardTemplateService(ICardTemplateRepository cardTemplateRepository)
    {
        _cardTemplateRepository = cardTemplateRepository;
    }

    public async Task<Guid> CreateAsync(CardTemplateDto templateDto, CancellationToken ct)
    {
        CardTemplate cardTemplate;
        if (templateDto.BackgroundImage.Id != Guid.Empty) 
            cardTemplate = new CardTemplate(templateDto.Title,
                templateDto.Description, 
                templateDto.MaxBonuses,
                templateDto.IsFreeFirstPoint,
                templateDto.CompanyId, 
                templateDto.BackgroundImage.Id);
        else
        {
            cardTemplate = new CardTemplate(templateDto.Title,
                templateDto.Description,
                templateDto.MaxBonuses,
                templateDto.IsFreeFirstPoint,
                templateDto.CompanyId);

            cardTemplate.UpdateImage(templateDto.BackgroundImage.Name, false);

            cardTemplate.BackgroundImageId = await _cardTemplateRepository
                .CreateBackgroundImage(cardTemplate.BackgroundImage, ct);
        }

        return await _cardTemplateRepository.Create(cardTemplate, ct);
    }

    public async Task<Guid> CreateImageAsync(string imageName, CancellationToken ct)
    {
        var newImage = new CardImage(true, imageName);
        return await _cardTemplateRepository
            .CreateBackgroundImage(newImage, ct);
    }

    public Task<Guid> UpdateAsync(CardTemplateDto templateDto, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<List<CardTemplateDto>> GetAllByCompanyIdAsync(Guid companyId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> DeleteAsync(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}