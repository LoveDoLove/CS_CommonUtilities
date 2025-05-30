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