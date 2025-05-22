using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace CommonUtilities.Utilities;

public static class LoggerUtilities
{
    public static void StartLog()
    {
        // Use Path.Combine for robust path construction
        string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "app_logs", ".log");
        // Consider a more descriptive log file name, e.g., "application.log" or "GitTools.log"
        // Also, ensure the "app_logs" directory exists or is created.
        // For simplicity, if ".log" is intended to be in the current directory:
        // string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "application.log");
        // If the intent was a hidden log file like ".log", ensure the directory part is handled.
        // For this example, I'll assume a log file in a subdirectory "logs".
        string logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        Directory.CreateDirectory(logDirectory); // Ensure log directory exists
        logFilePath = Path.Combine(logDirectory, "application.log");


        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "GitTools") // Adds app identifier to all logs
            .WriteTo.File(
                logFilePath, // Use the combined path
                fileSizeLimitBytes: 10_000_000, // 10MB size limit
                retainedFileCountLimit: 7, // Keep 7 days of logs
                rollingInterval: RollingInterval.Day,
                buffered: true, // Buffer writes for better performance
                flushToDiskInterval: TimeSpan.FromSeconds(5))
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Literate,
                restrictedToMinimumLevel: LogEventLevel.Information) // Only important info to console
            .CreateLogger();
    }

    public static void StopLog()
    {
        Log.CloseAndFlush(); // Ensure all buffered logs are written
    }
}