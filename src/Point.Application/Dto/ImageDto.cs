namespace Point.Application.Dto;

public class ImageDto
{
    public ImageDto(string imageUrl)
    {
        ImageUrl = imageUrl;
    }

    public string ImageUrl { get; set; }
}