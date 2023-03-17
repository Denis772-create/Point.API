namespace Point.Application.Interfaces;

public interface IQrCodeGenerator
{
    string CreateQrCode(string inputData);
}