namespace Point.Application.Dto;

public class ImageInput
{
    public IFormFile Photo { get; set; }
    public bool IsShared { get; set; } = false;
}