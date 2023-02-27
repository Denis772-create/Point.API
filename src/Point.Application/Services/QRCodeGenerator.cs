namespace Point.Application.Services;

public class QrCodeGenerator : IQrCodeGenerator
{
    public string CreateQrCode(string inputData)
    {
        // TODO: implement
        return Guid.NewGuid().ToString();
    }
}