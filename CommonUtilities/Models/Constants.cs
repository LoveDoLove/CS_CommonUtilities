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

namespace CommonUtilities.Models;

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