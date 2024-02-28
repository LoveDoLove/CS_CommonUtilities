using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace CommonUtilities.Utilities;

public static class LoggerUtilities
{
    public static void StartLog()
    {
        Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().MinimumLevel
            .Override("Microsoft", LogEventLevel.Warning).MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information).Enrich
            .FromLogContext().WriteTo
            .File(Directory.GetCurrentDirectory() + "/" + ".log", fileSizeLimitBytes: null,
                retainedFileCountLimit: null, rollingInterval: RollingInterval.Day).WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                theme: AnsiConsoleTheme.Literate).CreateLogger();
    }

    public static void StopLog()
    {
        Log.CloseAndFlush();
    }
}