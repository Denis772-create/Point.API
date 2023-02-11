namespace Point.Application.Services;

public class QrCodeGenerator : IQrCodeGenerator
{
    public string CreateQrCode(string inputData)
    {
        return Guid.NewGuid().ToString();
    }
}