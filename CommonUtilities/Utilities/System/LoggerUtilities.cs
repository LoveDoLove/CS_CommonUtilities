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

using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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
    ///     Builds a message prefix using the calling class and method names.
    /// </summary>
    private static string FormatMessage(string message)
    {
        try
        {
            (string className, string methodName) = GetCallerInfo();
            return $"[{className}] [{methodName}] {message}";
        }
        catch
        {
            // In case stack inspection fails for any reason, return the original message.
            return message;
        }
    }

    /// <summary>
    ///     Inspects the stack to find the first caller outside LoggerUtilities and Serilog internals.
    ///     Returns (ClassName, MethodName).
    /// </summary>
    private static (string ClassName, string MethodName) GetCallerInfo()
    {
        StackTrace stack = new StackTrace(1, false);
        foreach (StackFrame frame in stack.GetFrames())
        {
            MethodBase? method = frame.GetMethod();
            if (method == null) continue;

            Type? declaringType = method.DeclaringType;
            if (declaringType == null) continue;

            // Skip frames from this utility class
            if (declaringType == typeof(LoggerUtilities))
                continue;

            // Skip common Serilog frames
            string fullName = declaringType.FullName ?? string.Empty;
            if (fullName.StartsWith("Serilog.", StringComparison.Ordinal) ||
                fullName.StartsWith("Serilog", StringComparison.Ordinal))
                continue;

            // Handle compiler-generated state machine types (async/iterator) and closures
            // Common pattern: nested type like `<MethodName>d__N` with a MoveNext method.
            if (declaringType.IsNested && declaringType.Name.Contains("<"))
            {
                Type? outer = declaringType.DeclaringType;
                if (outer != null)
                {
                    // Try to find the original method by searching for AsyncStateMachine/IteratorStateMachine attributes
                    MethodInfo[] candidates = outer.GetMethods(BindingFlags.Instance | BindingFlags.Static |
                                                               BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (MethodInfo m in candidates)
                    {
                        AsyncStateMachineAttribute? asyncAttr = m.GetCustomAttribute<AsyncStateMachineAttribute>();
                        if (asyncAttr != null && asyncAttr.StateMachineType == declaringType)
                            return (outer.Name, m.Name);

                        IteratorStateMachineAttribute? iterAttr = m.GetCustomAttribute<IteratorStateMachineAttribute>();
                        if (iterAttr != null && iterAttr.StateMachineType == declaringType)
                            return (outer.Name, m.Name);
                    }

                    // Fallback: extract name from nested type like `<Index>d__2` -> Index
                    Match match = Regex.Match(declaringType.Name, "<(?<name>[^>]+)>");
                    if (match.Success)
                    {
                        string extracted = match.Groups["name"].Value;
                        return (outer.Name, extracted);
                    }

                    // As a last resort, use outer type name and MoveNext
                    return (outer.Name, method.Name);
                }
            }

            // If the declaring type is compiler-generated (closures) try to use declaringType.DeclaringType as class name
            if (declaringType.GetCustomAttribute<CompilerGeneratedAttribute>() != null ||
                declaringType.Name.Contains("DisplayClass") || declaringType.Name.Contains("AnonStorey"))
            {
                Type? outer = declaringType.DeclaringType;
                if (outer != null)
                    return (outer.Name, method.Name);
            }

            string className = declaringType.Name;
            string methodName = method.Name;
            return (className, methodName);
        }

        return ("UnknownClass", "UnknownMethod");
    }

    /// <summary>
    ///     Logs an informational message.
    /// </summary>
    public static void Info(string message)
    {
        Log.Information(FormatMessage(message));
    }

    /// <summary>
    ///     Logs an error message.
    /// </summary>
    public static void Error(string message)
    {
        Log.Error(FormatMessage(message));
    }

    /// <summary>
    ///     Logs a warning message.
    /// </summary>
    public static void Warning(string message)
    {
        Log.Warning(FormatMessage(message));
    }

    /// <summary>
    ///     Logs a debug message.
    /// </summary>
    public static void Debug(string message)
    {
        Log.Debug(FormatMessage(message));
    }

    /// <summary>
    ///     Logs an error message with exception.
    /// </summary>
    public static void Error(Exception exception, string message)
    {
        Log.Error(exception, FormatMessage(message));
    }

    /// <summary>
    ///     Logs a fatal message.
    /// </summary>
    public static void Fatal(string message)
    {
        Log.Fatal(FormatMessage(message));
    }

    /// <summary>
    ///     Logs a fatal message with exception.
    /// </summary>
    public static void Fatal(Exception exception, string message)
    {
        Log.Fatal(exception, FormatMessage(message));
    }
}