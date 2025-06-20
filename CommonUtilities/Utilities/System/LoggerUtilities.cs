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

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace CommonUtilities.Utilities.System;

/// <summary>
///     Provides static helpers for configuring and writing logs using Serilog.
/// </summary>
public static class LoggerUtilities
{
    /// <summary>
    ///     Starts Serilog logging with file and console sinks.
    /// </summary>
    /// <param name="applicationName">The name of the application for log file naming and enrichment. Required.</param>
    /// <param name="customLogFilePath">
    ///     Optional: Custom log file path. If null, defaults to %APPDATA%/
    ///     <applicationName>/<applicationName>.log
    /// </param>
    public static void StartLog(string applicationName, string? customLogFilePath = null)
    {
        if (string.IsNullOrWhiteSpace(applicationName))
            throw new ArgumentException("Application name must be provided.", nameof(applicationName));

        string logFilePath;
        if (!string.IsNullOrWhiteSpace(customLogFilePath))
        {
            string? customDir = Path.GetDirectoryName(customLogFilePath);
            if (!string.IsNullOrEmpty(customDir))
                Directory.CreateDirectory(customDir);
            logFilePath = customLogFilePath;
        }
        else
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string logDirectory = Path.Combine(appData, applicationName);
            Directory.CreateDirectory(logDirectory);
            logFilePath = Path.Combine(logDirectory, $"{applicationName}.log");
        }

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", applicationName)
            .WriteTo.File(
                logFilePath,
                fileSizeLimitBytes: 10_000_000,
                retainedFileCountLimit: 7,
                rollingInterval: RollingInterval.Day,
                buffered: true,
                flushToDiskInterval: TimeSpan.FromSeconds(5))
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Literate,
                restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();
    }

    /// <summary>
    ///     Stops Serilog logging and flushes all buffered logs.
    /// </summary>
    public static void StopLog()
    {
        Log.CloseAndFlush();
    }

    /// <summary>
    ///     Logs an informational message.
    /// </summary>
    public static void Info(string message)
    {
        Log.Information(message);
    }

    /// <summary>
    ///     Logs an error message.
    /// </summary>
    public static void Error(string message)
    {
        Log.Error(message);
    }

    /// <summary>
    ///     Logs a warning message.
    /// </summary>
    public static void Warning(string message)
    {
        Log.Warning(message);
    }

    /// <summary>
    ///     Logs a debug message.
    /// </summary>
    public static void Debug(string message)
    {
        Log.Debug(message);
    }

    /// <summary>
    ///     Logs an error message with exception.
    /// </summary>
    public static void Error(Exception exception, string message)
    {
        Log.Error(exception, message);
    }

    /// <summary>
    ///     Logs a fatal message.
    /// </summary>
    public static void Fatal(string message)
    {
        Log.Fatal(message);
    }

    /// <summary>
    ///     Logs a fatal message with exception.
    /// </summary>
    public static void Fatal(Exception exception, string message)
    {
        Log.Fatal(exception, message);
    }
}