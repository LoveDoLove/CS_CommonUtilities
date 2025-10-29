// Copyright 2025 LoveDoLove
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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