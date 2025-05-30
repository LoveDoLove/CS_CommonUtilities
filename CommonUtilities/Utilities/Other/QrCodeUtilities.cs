using QRCoder;

namespace CommonUtilities.Utilities.Other;

/// <summary>
///     Utility class for generating QR codes.
///     This class uses the QRCoder library to create QR codes from a given string (typically a URL)
///     and returns the QR code image as a Base64 encoded string.
/// </summary>
/// <remarks>
///     Author: LoveDoLove
/// </remarks>
public static class QrCodeUtilities
{
    /// <summary>
    ///     Generates a QR code image from the specified URL or text.
    /// </summary>
    /// <param name="content">The string content (e.g., URL, text) to encode in the QR code.</param>
    /// <returns>A Base64 encoded string representing the QR code image (PNG format by default from QRCoder's Base64QRCode).</returns>
    public static string GenerateQrCode(string content)
    {
        // Initialize the QR code generator
        QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();

        // Create QR code data with ECC Level Q (Quantile, ~25% correction)
        // ECCLevel.Q is a common choice balancing density and error correction.
        QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);

        // Generate the QR code as a Base64 string
        Base64QRCode qrCode = new Base64QRCode(qrCodeData);

        // Get the QR code graphic as a Base64 string.
        // The '20' parameter typically refers to the pixels per module (size of the QR code blocks).
        string qrCodeImageAsBase64 = qrCode.GetGraphic(20);

        return qrCodeImageAsBase64;
    }
}