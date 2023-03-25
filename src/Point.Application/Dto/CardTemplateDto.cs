
using Point.Application.Attributes;

namespace Point.Application.Dto;

public class CardTemplateDto
{
    [ContentValidation(50)]
    public string Title { get; set; }

    [ContentValidation(250)]
    public string Description { get; set; }

    [BonusValidation]
    public int MaxBonuses { get; set; }
    public bool IsFreeFirstPoint { get; set; }
    public Guid CompanyId { get; set; }
    public BackgroundImageDto BackgroundImage { get; set; }
}