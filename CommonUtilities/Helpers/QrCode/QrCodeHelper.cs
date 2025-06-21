// MIT License
// 
// Copyright (c) 2025 LoveDoLove
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using QRCoder;

namespace CommonUtilities.Helpers.QrCode;

/// <summary>
///     Utility class for generating QR codes.
///     This class uses the QRCoder library to create QR codes from a given string (typically a URL)
///     and returns the QR code image as a Base64 encoded string.
/// </summary>
/// <remarks>
///     Author: LoveDoLove
/// </remarks>
public static class QrCodeHelper
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