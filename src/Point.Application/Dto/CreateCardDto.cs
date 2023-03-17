namespace Point.Application.Dto;

public class CreateCardDto
{
    public Guid TemplateId { get; set; }
    public Guid UserId { get; set; }
}