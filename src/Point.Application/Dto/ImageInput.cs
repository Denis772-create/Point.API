namespace Point.Application.Dto;

public class ImageInput
{
    public ImageInput(IFormFile photo)
    {
        Photo = photo;
    }

    public IFormFile Photo { get; set; }
}