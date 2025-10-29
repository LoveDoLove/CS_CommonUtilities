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