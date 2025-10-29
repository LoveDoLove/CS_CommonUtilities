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

using Microsoft.Extensions.Configuration;

// Required for Path and FileNotFoundException

namespace CommonUtilities.Utilities.System;

/// <summary>
///     Provides utility methods for accessing application settings.
/// </summary>
public static class AppSettingsUtilities
{
    private static readonly Lazy<IConfiguration> LazyConfiguration = new(AppSettingsInit);

    /// <summary>
    ///     Gets the application configuration.
    /// </summary>
    public static IConfiguration Configuration => LazyConfiguration.Value;

    /// <summary>
    ///     Initializes the application configuration by reading from "appsettings.json".
    /// </summary>
    /// <returns>An IConfiguration object representing the application settings.</returns>
    /// <exception cref="FileNotFoundException">Thrown if "appsettings.json" is not found.</exception>
    private static IConfiguration AppSettingsInit()
    {
        // Use Path.Combine for robust path construction
        string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        if (!File.Exists(appSettingsPath))
            // Throw an exception instead of exiting the application
            throw new FileNotFoundException("appsettings.json not found!", appSettingsPath);

        return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json")
            .Build();
    }
}