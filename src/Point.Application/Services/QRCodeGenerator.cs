namespace Point.Application.Services;

public class QrCodeGenerator : IQrCodeGenerator
{
    public string CreateQrCode(string inputData)
    {
        var qrGenerator = new QrCodeGenerator();
        return qrGenerator.CreateQrCode(inputData); 
    }
}