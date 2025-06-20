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

namespace CommonUtilities.Utilities.Security;

/// <summary>
///     Provides utility methods for validation.
/// </summary>
public static class ValidationUtilities
{
    /// <summary>
    ///     Checks if a string is a valid hexadecimal representation.
    /// </summary>
    /// <param name="szText">The string to validate.</param>
    /// <returns>True if the string is a valid hex string, false otherwise.</returns>
    public static bool IsValidHex(string szText)
    {
        if (string.IsNullOrEmpty(szText))
            return false;

        // Hex strings often have an even length (each byte is two hex chars).
        // This check can be added if strictness is required:
        // if (szText.Length % 2 != 0)
        // return false;

        foreach (char c in szText)
        {
            bool isHexChar = (c >= '0' && c <= '9') ||
                             (c >= 'a' && c <= 'f') ||
                             (c >= 'A' && c <= 'F');
            if (!isHexChar)
                return false;
        }

        return true;
    }
}