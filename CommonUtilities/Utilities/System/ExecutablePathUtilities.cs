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

namespace CommonUtilities.Utilities.System;

/// <summary>
///     Provides utility methods for finding executable files.
/// </summary>
public static class ExecutablePathUtilities
{
    /// <summary>
    ///     Searches the system's PATH environment variable for a specific executable.
    /// </summary>
    /// <param name="executableName">The name of the executable to find (e.g., "git", "npm").</param>
    /// <returns>The full path to the executable if found; otherwise, null.</returns>
    public static string? FindInPath(string executableName)
    {
        if (string.IsNullOrWhiteSpace(executableName))
        {
            LoggerUtilities.Info("FindInPath called with null or empty executableName.");
            return null;
        }

        // Handle cases where executableName might already have an extension (common on Windows when users type it)
        // or if it's a full path already.
        if (File.Exists(executableName)) return Path.GetFullPath(executableName);

        var pathVariable = Environment.GetEnvironmentVariable("PATH");
        if (string.IsNullOrEmpty(pathVariable))
        {
            LoggerUtilities.Info("PATH environment variable is not set or is empty.");
            return null;
        }

        var paths = pathVariable.Split(Path.PathSeparator);
        var extensions = GetExecutableExtensions();

        // Ensure we check for the name as-is first (for Unix-like systems or if extension is included)
        var extensionsToSearch = new List<string> { string.Empty };
        extensionsToSearch.AddRange(extensions.Where(ext => !string.IsNullOrEmpty(ext))); // Add OS-specific extensions

        foreach (var pathDir in paths)
        {
            if (string.IsNullOrWhiteSpace(pathDir) || !Directory.Exists(pathDir)) continue;

            foreach (var ext in extensionsToSearch)
            {
                var potentialPath = Path.Combine(pathDir.Trim(), executableName + ext);
                try
                {
                    if (File.Exists(potentialPath)) return Path.GetFullPath(potentialPath);
                }
                catch (Exception ex) // Catch potential issues like permission errors
                {
                    LoggerUtilities.Error(ex, $"Error checking file existence for {potentialPath}");
                    // Continue to next path
                }
            }
        }

        LoggerUtilities.Info($"Executable '{executableName}' not found in PATH.");
        return null;
    }

    /// <summary>
    ///     Gets common executable extensions based on the operating system.
    /// </summary>
    /// <returns>An array of executable extensions (e.g., {".exe", ".cmd", ".bat"} for Windows, empty for Unix-like systems).</returns>
    public static string[] GetExecutableExtensions()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            return new[] { ".exe", ".cmd", ".bat", ".com", ".ps1" }; // Added .com and .ps1 for completeness on Windows
        // For Unix-like systems, executables often don't have extensions,
        // or they might be scripts like .sh, .py, etc.
        // Returning an empty string in the array allows checking for the name as-is.
        // Specific script extensions can be added if a more comprehensive search is needed,
        // but for general executables, this is often sufficient.
        return new[] { string.Empty }; // For non-Windows, primarily check without extension
    }

    /// <summary>
    ///     Attempts to find an executable by checking an explicitly configured path, then the system PATH, and then additional
    ///     provided search directories.
    /// </summary>
    /// <param name="executableName">The name of the executable (e.g., "node", "php").</param>
    /// <param name="configuredPath">
    ///     An explicitly configured path for the executable. If provided and valid, it will be used
    ///     directly.
    /// </param>
    /// <param name="additionalSearchDirectories">
    ///     A collection of additional directories to search if not found in
    ///     configuredPath or system PATH.
    /// </param>
    /// <returns>The full path to the executable if found; otherwise, null.</returns>
    public static string? FindExecutable(string executableName, string? configuredPath = null,
        IEnumerable<string>? additionalSearchDirectories = null)
    {
        if (string.IsNullOrWhiteSpace(executableName))
        {
            LoggerUtilities.Info("FindExecutable called with null or empty executableName.");
            return null;
        }

        // 1. Check configuredPath
        if (!string.IsNullOrWhiteSpace(configuredPath))
            try
            {
                if (File.Exists(configuredPath))
                {
                    LoggerUtilities.Info($"Executable '{executableName}' found at configured path: {configuredPath}");
                    return Path.GetFullPath(configuredPath);
                }

                LoggerUtilities.Info(
                    $"Configured path '{configuredPath}' for '{executableName}' does not exist or is not a file.");
            }
            catch (Exception ex)
            {
                LoggerUtilities.Error(ex, $"Error checking configured path {configuredPath}");
            }

        // 2. Search in system PATH
        string? pathFromEnv = FindInPath(executableName);
        if (!string.IsNullOrWhiteSpace(pathFromEnv))
        {
            LoggerUtilities.Info($"Executable '{executableName}' found in system PATH: {pathFromEnv}");
            return pathFromEnv; // Already a full path
        }

        // 3. Search in additionalSearchDirectories
        if (additionalSearchDirectories != null)
        {
            var extensions = GetExecutableExtensions();
            // Ensure we check for the name as-is first (for Unix-like systems or if extension is included)
            var extensionsToSearch = new List<string> { string.Empty };
            extensionsToSearch.AddRange(extensions.Where(ext => !string.IsNullOrEmpty(ext)));


            foreach (var dir in additionalSearchDirectories)
            {
                if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
                {
                    LoggerUtilities.Debug($"Skipping invalid additional search directory: {dir}");
                    continue;
                }

                foreach (var ext in extensionsToSearch)
                {
                    var potentialPath = Path.Combine(dir.Trim(), executableName + ext);
                    try
                    {
                        if (File.Exists(potentialPath))
                        {
                            LoggerUtilities.Info(
                                $"Executable '{executableName}' found in additional directory '{dir}': {potentialPath}");
                            return Path.GetFullPath(potentialPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerUtilities.Error(ex,
                            $"Error checking file existence for {potentialPath} in additional directory {dir}");
                    }
                }
            }

            LoggerUtilities.Info($"Executable '{executableName}' not found in additional search directories.");
        }

        LoggerUtilities.Info(
            $"Executable '{executableName}' not found after checking configured path, system PATH, and additional directories.");
        return null;
    }
}