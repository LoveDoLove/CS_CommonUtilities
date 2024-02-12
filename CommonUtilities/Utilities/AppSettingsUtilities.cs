using Microsoft.Extensions.Configuration;

namespace CommonUtilities.Utilities;

public class AppSettingsUtilities
{
    private static readonly Lazy<IConfiguration> LazyConfiguration = new(AppSettingsInit);

    public static IConfiguration Configuration => LazyConfiguration.Value;

    private static IConfiguration AppSettingsInit()
    {
        string appSettingsPath = Directory.GetCurrentDirectory() + "/appsettings.json";
        if (!File.Exists(appSettingsPath))
        {
            Console.Error.WriteLine("appsettings.json not found!");
            Environment.Exit(-1);
        }

        return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json")
            .Build();
    }
}