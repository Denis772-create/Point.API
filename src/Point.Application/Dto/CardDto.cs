namespace Point.Application.Dto;

public class CardDto
{
    public CardTemplateDto Template { get; set; }
    public bool IsActive { get; set; }
    public int CountSteps { get; set; }
    public string CardNumber { get; set; }

    public static CardDto FromCard(Card card)
        => new()
        {
            CardNumber = card.CardNumber,
            CountSteps = card.CountSteps,
            IsActive = card.IsActive,
            Template = new CardTemplateDto
            {
                BackgroundImage = new BackgroundImageDto
                {
                    Id = card.CardTemplate.BackgroundImage.Id,
                    Name = card.CardTemplate.BackgroundImage.Name,
                },
                Description = card.CardTemplate.Description,
                MaxBonuses = card.CardTemplate.MaxBonuses,
                Title = card.CardTemplate.Title
            }
        };
}