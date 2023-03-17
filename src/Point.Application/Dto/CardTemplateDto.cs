namespace Point.Application.Dto;

public class CardTemplateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int MaxBonuses { get; set; }
    public bool IsFreeFirstPoint { get; set; }
    public Guid CompanyId { get; set; }
    public BackgroundImageDto BackgroundImage { get; set; }
}