using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Application.Utilities;

public class LoggerUtilities
{
    public static void StartLog()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File(Directory.GetCurrentDirectory() + "/" + DateTime.Now.ToString("yyyyMMdd") + ".log",
                fileSizeLimitBytes: null,
                retainedFileCountLimit: null)
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                theme: AnsiConsoleTheme.Literate)
            .CreateLogger();
    }

    public static void StopLog()
    {
        Log.CloseAndFlush();
    }
}