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

namespace CommonUtilities.Models.Share;

/// <summary>
///     Provides various constants used throughout the application.
/// </summary>
public class Constants
{
    // WARNING: Using a fixed IV like this for cryptographic operations (e.g., AES-CBC, TripleDES-CBC)
    // is insecure if used for multiple messages with the same key.
    // IVs should ideally be unique and randomly generated for each encryption operation.
    // This default might be suitable for specific, controlled scenarios or testing.
    /// <summary>
    ///     Default Initialization Vector (IV) for cryptographic operations.
    ///     <remarks>
    ///         Using a fixed IV is insecure for multiple messages with the same key.
    ///         It should ideally be unique and randomly generated for each encryption.
    ///         This default may be suitable for specific, controlled scenarios or testing.
    ///     </remarks>
    /// </summary>
    public const string DefaultIv = "0000000000000000";

    /// <summary>
    ///     Maximum allowed length for a URL.
    /// </summary>
    public const int MaxUrlLength = 100;

    /// <summary>
    ///     A string used as a separator line in console output.
    /// </summary>
    public const string ConsoleSplitLine =
        "------------------------------------------------------------------------------------";
}