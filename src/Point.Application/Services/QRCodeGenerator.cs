using Image = SixLabors.ImageSharp.Image;

namespace Point.Application.Services;

public class QrCodeGenerator : IQrCodeGenerator
{
    private readonly QrCodeOptions _options;

    public QrCodeGenerator(IOptions<QrCodeOptions> options)
    {
        _options = options.Value;
    }

    public string CreateQrCode(string inputData)
    {
        var barcodeWriter = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = _options.Height,
                Width = _options.Width
            }
        };

        var pixelData = barcodeWriter.Write(inputData);
        using var image = Image.LoadPixelData<Rgba32>(pixelData.Pixels, pixelData.Width, pixelData.Height);
        image.Mutate(x => x.Resize(_options.Width, _options.Height));

        using var stream = new MemoryStream();
        image.Save(stream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());

        return Convert.ToBase64String(stream.ToArray());
    }
}