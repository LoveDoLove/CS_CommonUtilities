using QRCoder;

namespace CommonUtilities.Helpers;

public static class QrCodeHelper
{
    public static string GenerateQrCode(string url)
    {
        QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        Base64QRCode qrCode = new Base64QRCode(qrCodeData);
        string qrCodeImageAsBase64 = qrCode.GetGraphic(20);
        return qrCodeImageAsBase64;
    }
}