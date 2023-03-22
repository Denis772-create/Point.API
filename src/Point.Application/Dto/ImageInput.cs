using Point.Application.Attributes;

namespace Point.Application.Dto;

public class ImageInput
{
    public ImageInput(IFormFile photo)
    {
        Photo = photo;
    }

    [ImageValidation]
    public IFormFile Photo { get; set; }
}