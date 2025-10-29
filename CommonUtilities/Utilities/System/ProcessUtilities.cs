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

using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonUtilities.Utilities.System;

/// <summary>
///     Provides static utilities for process execution, diagnostics, and environment checks.
/// </summary>
public static class ProcessUtilities
{
    /// <summary>
    ///     Runs a process asynchronously and returns output, error, and exit code.
    /// </summary>
    public static async Task<(string output, string error, int exitCode)> RunProcessAsync(
        string fileName,
        string arguments,
        string? workingDirectory = null,
        int timeoutMilliseconds = 300000, // Default timeout: 5 minutes
        CancellationToken cancellationToken = default)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = true, // Capture standard output
            RedirectStandardError = true, // Capture standard error
            UseShellExecute = false, // Do not use OS shell
            CreateNoWindow = true // Do not create a window
        };

        // Set working directory if provided
        if (!string.IsNullOrEmpty(workingDirectory)) process.StartInfo.WorkingDirectory = workingDirectory;

        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        // Asynchronously read standard output
        process.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) outputBuilder.AppendLine(e.Data);
        };
        // Asynchronously read standard error
        process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) errorBuilder.AppendLine(e.Data);
        };

        try
        {
            process.Start();
            process.BeginOutputReadLine(); // Start reading output
            process.BeginErrorReadLine(); // Start reading error

            // Wait for the process to exit or for the timeout to occur or for cancellation
            var processTask = process.WaitForExitAsync(cancellationToken);
            var timeoutTask = Task.Delay(timeoutMilliseconds, cancellationToken);

            // If timeout occurs before the process exits
            if (await Task.WhenAny(processTask, timeoutTask) == timeoutTask)
                if (!process.HasExited) // Check if the process is still running
                {
                    try
                    {
                        // Attempt to kill the process and its child processes
                        process.Kill(true);
                    }
                    catch (InvalidOperationException)
                    {
                        /* Process already exited */
                    }
                    catch (Win32Exception)
                    {
                        /* Process could not be terminated or is exiting */
                    }

                    // It's important to catch specific exceptions here to avoid masking other issues.
                    throw new TimeoutException(
                        $"Process timed out after {timeoutMilliseconds / 1000} seconds: {fileName} {arguments}");
                }

            // If process exited or was cancelled, ensure it's fully completed by awaiting the process task.
            // This also rethrows OperationCanceledException if cancellationToken was triggered.
            await processTask;

            return (outputBuilder.ToString(), errorBuilder.ToString(), process.ExitCode);
        }
        catch (OperationCanceledException) // Handle cancellation explicitly
        {
            if (!process.HasExited)
                try
                {
                    process.Kill(true);
                }
                catch (InvalidOperationException)
                {
                    /* Process already exited */
                }
                catch (Win32Exception)
                {
                    /* Process could not be terminated or is exiting */
                }

            throw; // Re-throw cancellation exception
        }
        catch (Win32Exception ex) // Handle errors like "file not found" or permission issues
        {
            // Provide a more specific error message for Win32Exceptions
            throw new InvalidOperationException(
                $"Error starting process '{fileName}'. Ensure the command is correct and accessible. Details: {ex.Message}",
                ex);
        }
        catch (Exception ex) // Catch-all for other unexpected errors during process execution
        {
            if (!process.HasExited)
                try
                {
                    process.Kill(true);
                }
                catch (InvalidOperationException)
                {
                    /* Process already exited */
                }
                catch (Win32Exception)
                {
                    /* Process could not be terminated or is exiting */
                }

            // Wrap the original exception to provide context
            throw new Exception(
                $"An unexpected error occurred while running process '{fileName} {arguments}'. Details: {ex.Message}",
                ex);
        }
    }

    /// <summary>
    ///     Runs a process synchronously and returns output, error, and exit code.
    /// </summary>
    /// <param name="fileName">The name or path of the executable to run.</param>
    /// <param name="arguments">The arguments to pass to the executable.</param>
    /// <param name="workingDirectory">Optional working directory for the process.</param>
    /// <returns>A tuple containing the standard output, standard error, and exit code of the process.</returns>
    public static (string output, string error, int exitCode) RunProcess(
        string fileName,
        string arguments,
        string? workingDirectory = null)
    {
        var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        if (!string.IsNullOrEmpty(workingDirectory)) process.StartInfo.WorkingDirectory = workingDirectory;

        process.Start();
        // Read output and error streams synchronously
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit(); // Wait for the process to complete

        return (output, error, process.ExitCode);
    }

    /// <summary>
    ///     Finds the explicit path for a given executable name by searching common system locations.
    /// </summary>
    /// <param name="executableName">The name of the executable (e.g., "php", "npm").</param>
    /// <returns>The full path to the executable if found; otherwise, null.</returns>
    public static string? FindExecutablePath(string executableName)
    {
        string[] searchPaths = GetSearchPathsForExecutable(executableName);
        foreach (var path in searchPaths)
            if (File.Exists(path))
                return path;

        return null; // Executable not found in common locations
    }

    /// <summary>
    ///     Gets an array of potential paths where an executable might be found, based on the operating system
    ///     and common installation directories for specific tools.
    /// </summary>
    /// <param name="executableName">The name of the executable (e.g., "php", "node").</param>
    /// <returns>An array of potential full paths to the executable.</returns>
    private static string[] GetSearchPathsForExecutable(string executableName)
    {
        // Determine appropriate file extension based on OS
        string extension = Environment.OSVersion.Platform == PlatformID.Win32NT ? ".exe" : "";
        var paths = new List<string>();
        string lowerExecutableName = executableName.ToLowerInvariant();

        if (Environment.OSVersion.Platform == PlatformID.Win32NT) // Windows-specific paths
        {
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string appData =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // For user-specific installations

            switch (lowerExecutableName)
            {
                case "npm":
                case "node":
                    paths.Add(Path.Combine(programFiles, "nodejs", $"{executableName}{extension}"));
                    paths.Add(Path.Combine(programFilesX86, "nodejs",
                        $"{executableName}{extension}")); // For 32-bit Node.js on 64-bit Windows
                    paths.Add(Path.Combine(appData, "npm", $"{executableName}.cmd")); // npm often uses .cmd on Windows
                    paths.Add(Path.Combine(appData, "npm",
                        executableName)); // Sometimes it's just 'npm' without extension in PATH
                    break;
                case "php":
                    paths.Add(Path.Combine(programFiles, "PHP", $"{executableName}{extension}"));
                    paths.Add(Path.Combine(programFilesX86, "PHP", $"{executableName}{extension}"));
                    // Common XAMPP/WAMP paths
                    paths.Add(Path.Combine("C:\\", "xampp", "php", $"{executableName}{extension}"));
                    paths.Add(Path.Combine("C:\\", "Program Files", "php",
                        $"{executableName}{extension}")); // Generic PHP install
                    break;
                case "composer":
                    // Composer paths can vary, these are common ones
                    paths.Add(Path.Combine(appData, "Composer", "vendor", "bin", "composer.bat"));
                    paths.Add(Path.Combine(appData, "Composer", "vendor", "bin",
                        "composer")); // For git bash or similar
                    paths.Add(Path.Combine("C:\\", "ProgramData", "ComposerSetup", "bin",
                        "composer.bat")); // System-wide install
                    paths.Add(Path.Combine(appData, "Composer", "composer.phar")); // Direct phar path
                    break;
            }
        }
        else // Linux/macOS specific paths
        {
            // string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); // User's home directory
            switch (lowerExecutableName)
            {
                case "npm":
                case "node":
                    paths.Add(Path.Combine("/usr", "local", "bin", executableName)); // Common global install location
                    paths.Add(Path.Combine("/usr", "bin", executableName)); // System binary location
                    // Consider paths for version managers like nvm if more robustness is needed:
                    // e.g., Path.Combine(home, ".nvm", "versions", "node", "current_version_dir", "bin", executableName)
                    break;
                case "php":
                    paths.Add(Path.Combine("/usr", "local", "bin", executableName));
                    paths.Add(Path.Combine("/usr", "bin", executableName));
                    paths.Add(Path.Combine("/opt", "homebrew", "bin",
                        executableName)); // For Homebrew on Apple Silicon macOS
                    break;
                case "composer":
                    paths.Add(Path.Combine("/usr", "local", "bin", executableName)); // Often a symlink to composer.phar
                    paths.Add(Path.Combine("/usr", "bin", executableName));
                    // paths.Add(Path.Combine(home, ".composer", "vendor", "bin", executableName)); // User-specific global install
                    break;
            }
        }

        // Also add the executable name directly to search in PATH (OS will resolve this if it's in PATH)
        paths.Add(executableName +
                  (Environment.OSVersion.Platform == PlatformID.Win32NT && !lowerExecutableName.EndsWith(".exe") &&
                   !lowerExecutableName.EndsWith(".bat") && !lowerExecutableName.EndsWith(".cmd")
                      ? extension
                      : ""));
        paths.Add(executableName); // For non-Windows or when extension is already part of the name

        return paths.Distinct().ToArray(); // Return unique potential paths
    }

    /// <summary>
    ///     Gets PHP version information including the version string, path to the executable, and compatibility status.
    ///     Compatibility is typically defined as PHP 7.3 or higher.
    /// </summary>
    /// <returns>A tuple: (string version, string path, bool isCompatible).</returns>
    public static (string version, string path, bool isCompatible) GetPhpVersionInfo()
    {
        string phpPath = "php"; // Default to assuming 'php' is in PATH
        string version = "Unknown";
        bool isCompatible = false;

        // Attempt to find a more specific path for PHP
        var explicitPath = FindExecutablePath("php");
        if (!string.IsNullOrEmpty(explicitPath)) phpPath = explicitPath;

        var (output, _, exitCode) = RunProcess(phpPath, "--version");
        if (exitCode == 0 && !string.IsNullOrWhiteSpace(output))
        {
            // Regex to capture PHP version (e.g., "PHP 8.1.2 (cli)..." or "PHP 7.4.33 ...")
            var versionMatch = Regex.Match(output, @"PHP\s+(\d+\.\d+\.\d+)");
            if (versionMatch.Success)
            {
                version = versionMatch.Groups[1].Value;
                var versionParts = version.Split('.');
                // Check for major version 7 and minor version 3 or higher, or major version > 7
                if (versionParts.Length >= 2 &&
                    int.TryParse(versionParts[0], out int major) &&
                    int.TryParse(versionParts[1], out int minor))
                    isCompatible = major > 7 || (major == 7 && minor >= 3);
            }
        }

        return (version, phpPath, isCompatible);
    }

    /// <summary>
    ///     Checks if required PHP extensions are installed and determines overall compatibility.
    ///     Critical extensions for compatibility include JSON, PDO, OpenSSL, and Mbstring.
    /// </summary>
    /// <param name="phpPath">The path to the PHP executable. Defaults to "php" (assumes it's in PATH).</param>
    /// <returns>A tuple: (List<string> missingExtensions, bool isCompatible).</returns>
    public static (List<string> missingExtensions, bool isCompatible) CheckPhpExtensions(string phpPath = "php")
    {
        var requiredExtensions = new List<string>
        {
            "BCMath", "Ctype", "Fileinfo", "JSON", "Mbstring", "OpenSSL", "PDO", "Tokenizer", "XML", "cURL"
            // Add or remove extensions as per specific project requirements
        };
        var missingExtensions = new List<string>();
        bool isCompatible = true; // Assume compatible until a critical extension is found missing

        // Run 'php -m' to list loaded modules
        var (output, _, exitCode) = RunProcess(phpPath, "-m");
        if (exitCode == 0 && !string.IsNullOrWhiteSpace(output))
        {
            // Parse the output to get a list of loaded extensions.
            // Output of 'php -m' lists modules, one per line, sometimes with [Zend Modules] sections.
            var loadedExtensions = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ext => ext.Trim().ToLowerInvariant()) // Use ToLowerInvariant for case-insensitive comparison
                .Where(ext =>
                    !string.IsNullOrWhiteSpace(ext) && !ext.StartsWith("[") &&
                    !ext.Contains(" ")) // Filter out section headers and empty lines
                .ToList();

            foreach (var reqExt in requiredExtensions)
                if (!loadedExtensions.Contains(reqExt.ToLowerInvariant()))
                    missingExtensions.Add(reqExt);

            // Define critical extensions whose absence makes the setup incompatible for many web apps.
            var criticalExtensions = new HashSet<string> { "json", "pdo", "openssl", "mbstring" };
            if (missingExtensions.Any(ext => criticalExtensions.Contains(ext.ToLowerInvariant()))) isCompatible = false;
        }
        else
        {
            // If 'php -m' fails or returns no output, assume incompatibility and all extensions are missing.
            isCompatible = false;
            missingExtensions.AddRange(requiredExtensions); // Report all as missing
        }

        return (missingExtensions, isCompatible);
    }

    /// <summary>
    ///     Gets Composer version information, its path, and whether it's globally installed or a local PHAR.
    /// </summary>
    /// <returns>A tuple: (string version, string path, bool isGlobal).</returns>
    public static (string version, string path, bool isGlobal) GetComposerInfo()
    {
        string version = "Unknown";
        string composerExecutable = "composer"; // Default to 'composer' (expected in PATH)
        string argumentsToUse = "--version";
        bool isGlobal = true; // Assume global if "composer" is found in PATH

        // Attempt to find an explicit path for composer
        var explicitPath = FindExecutablePath("composer");

        if (!string.IsNullOrEmpty(explicitPath))
        {
            composerExecutable = explicitPath;
            // If the explicit path is a .phar file, it needs to be run with PHP.
            if (explicitPath.EndsWith(".phar", StringComparison.OrdinalIgnoreCase))
            {
                argumentsToUse = $"{explicitPath} --version"; // e.g., "C:\path\to\composer.phar --version"
                composerExecutable = "php"; // The actual command to run is PHP
                isGlobal = false; // Typically a direct .phar execution is considered local or specific.
            }
            // If it's a .bat or direct executable, it's likely a global setup.
        }
        else if (File.Exists("composer.phar")) // Check for local composer.phar as a fallback
        {
            argumentsToUse = "composer.phar --version";
            composerExecutable = "php";
            isGlobal = false; // Local .phar is definitely not global in the PATH sense
        }
        // If neither explicitPath nor local composer.phar is found, 'composer --version' will be tried.
        // If 'composer' is not in PATH, RunProcess will likely fail (which is handled).

        var (output, _, exitCode) = RunProcess(composerExecutable, argumentsToUse);

        if (exitCode == 0 && !string.IsNullOrWhiteSpace(output))
        {
            // Regex to capture Composer version (e.g., "Composer version 2.1.9 ...")
            var versionMatch = Regex.Match(output, @"Composer version (\d+\.\d+\.\d+)");
            if (versionMatch.Success) version = versionMatch.Groups[1].Value;
        }

        // Determine the final path string to return
        string finalPath = composerExecutable;
        if (composerExecutable == "php" && argumentsToUse.Contains(".phar"))
        {
            // Extract the phar path from arguments if PHP was used
            var pharPathMatch = Regex.Match(argumentsToUse, @"(^.*?\.phar)");
            if (pharPathMatch.Success) finalPath = $"php {pharPathMatch.Groups[1].Value}";
        }
        else if (composerExecutable == "composer" && !string.IsNullOrEmpty(explicitPath))
        {
            finalPath = explicitPath; // Use the found explicit path
        }

        return (version, finalPath, isGlobal);
    }

    /// <summary>
    ///     Gets Laravel version information (version, compatibility) by running 'php artisan --version'
    ///     in the specified or current working directory.
    ///     Compatibility is typically defined as Laravel 6 or higher.
    /// </summary>
    /// <param name="workingDirectory">Optional path to the Laravel project directory. Defaults to current directory.</param>
    /// <returns>A tuple: (string version, bool isCompatible).</returns>
    public static (string version, bool isCompatible) GetLaravelVersionInfo(string? workingDirectory = null)
    {
        string version = "Unknown";
        bool isCompatible = false;
        string projectDirectory = workingDirectory ?? Environment.CurrentDirectory;

        // Check if 'artisan' file exists in the directory, which is a good indicator of a Laravel project.
        if (!File.Exists(Path.Combine(projectDirectory, "artisan")))
            return (version, isCompatible); // Not a Laravel project or artisan is missing.

        var (output, _, exitCode) = RunProcess("php", "artisan --version", projectDirectory);
        if (exitCode == 0 && !string.IsNullOrWhiteSpace(output))
        {
            // Regex to capture Laravel Framework version (e.g., "Laravel Framework 9.52.7")
            var versionMatch = Regex.Match(output, @"Laravel Framework\s+(\d+\.\d+\.?\d*)");
            if (versionMatch.Success)
            {
                version = versionMatch.Groups[1].Value;
                if (version.Contains('.')) // Ensure version string is valid
                {
                    string majorVersionStr = version.Split('.')[0];
                    if (int.TryParse(majorVersionStr, out int majorVersion))
                        // Define compatibility (e.g., Laravel 6 and above)
                        isCompatible = majorVersion >= 6;
                }
            }
        }

        return (version, isCompatible);
    }
}