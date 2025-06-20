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

using System.ComponentModel;
using System.Diagnostics;

namespace CommonUtilities.Utilities.System;

/// <summary>
///     Provides utility methods for executing external processes.
/// </summary>
public static class ProcessExecutionUtilities
{
    /// <summary>
    ///     Runs a process and captures its standard output, standard error, and exit code.
    ///     This method does not provide real-time output.
    /// </summary>
    /// <param name="executablePath">The full path to the executable file to run.</param>
    /// <param name="arguments">The arguments to pass to the executable.</param>
    /// <param name="workingDirectory">The working directory for the process. Defaults to the current directory if null.</param>
    /// <param name="loadUserProfile">
    ///     For Windows, whether to load the user's profile. Defaults to true if OS is Windows, false
    ///     otherwise.
    /// </param>
    /// <returns>A tuple containing (string output, string error, int exitCode).</returns>
    public static (string output, string error, int exitCode) RunProcessAndCaptureOutput(string executablePath,
        string arguments, string? workingDirectory = null, bool? loadUserProfile = null)
    {
        string output = string.Empty;
        string error = string.Empty;
        int exitCode = -1;

        bool actualLoadUserProfile = loadUserProfile ?? Environment.OSVersion.Platform == PlatformID.Win32NT;

        try
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = executablePath;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    process.StartInfo.LoadUserProfile = actualLoadUserProfile;

                process.Start();

                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();

                process.WaitForExit();
                exitCode = process.ExitCode;
            }
        }
        catch (Win32Exception ex)
        {
            string message =
                $"Win32Exception (NativeErrorCode: {ex.NativeErrorCode}) while trying to run '{executablePath} {arguments}'. Message: {ex.Message}";
            if (ex.NativeErrorCode == 2) // ERROR_FILE_NOT_FOUND
                message =
                    $"Executable not found: '{executablePath}'. Ensure it is correctly installed and in PATH or configured path.";
            LoggerUtilities.Error(ex, message);
            error = message; // Capture the error message
            exitCode = ex.NativeErrorCode != 0 ? ex.NativeErrorCode : -1; // Use NativeErrorCode if available
        }
        catch (Exception ex)
        {
            string message = $"Exception while trying to run '{executablePath} {arguments}'. Message: {ex.Message}";
            LoggerUtilities.Error(ex, message);
            error = message; // Capture the error message
            exitCode = -1; // General error
        }

        return (output, error, exitCode);
    }

    /// <summary>
    ///     Runs a process, optionally streaming its standard output and standard error.
    /// </summary>
    /// <param name="executablePath">The full path to the executable file to run.</param>
    /// <param name="arguments">The arguments to pass to the executable.</param>
    /// <param name="workingDirectory">The working directory for the process. Defaults to the current directory if null.</param>
    /// <param name="loadUserProfile">
    ///     For Windows, whether to load the user's profile. Defaults to true if OS is Windows, false
    ///     otherwise.
    /// </param>
    /// <param name="outputDataReceived">Optional callback for standard output lines.</param>
    /// <param name="errorDataReceived">Optional callback for standard error lines.</param>
    /// <returns>The exit code of the process.</returns>
    public static int RunProcessWithStreaming(string executablePath, string arguments, string? workingDirectory = null,
        bool? loadUserProfile = null, Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null)
    {
        bool actualLoadUserProfile = loadUserProfile ?? Environment.OSVersion.Platform == PlatformID.Win32NT;
        int exitCode = -1;

        try
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = executablePath;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    process.StartInfo.LoadUserProfile = actualLoadUserProfile;

                if (outputDataReceived != null)
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data != null) outputDataReceived(e.Data);
                    };
                else
                    // Consume output to prevent buffer issues if no callback is provided
                    process.OutputDataReceived += (sender, e) => { _ = e.Data; };

                if (errorDataReceived != null)
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null) errorDataReceived(e.Data);
                    };
                else
                    // Consume error to prevent buffer issues if no callback is provided
                    process.ErrorDataReceived += (sender, e) => { _ = e.Data; };

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();
                exitCode = process.ExitCode;
            }
        }
        catch (Win32Exception ex)
        {
            string message =
                $"Win32Exception (NativeErrorCode: {ex.NativeErrorCode}) while trying to run '{executablePath} {arguments}'. Message: {ex.Message}";
            if (ex.NativeErrorCode == 2) // ERROR_FILE_NOT_FOUND
                message =
                    $"Executable not found: '{executablePath}'. Ensure it is correctly installed and in PATH or configured path.";
            LoggerUtilities.Error(ex, message);
            errorDataReceived?.Invoke(message); // Send error to callback if available
            exitCode = ex.NativeErrorCode != 0 ? ex.NativeErrorCode : -1;
        }
        catch (Exception ex)
        {
            string message = $"Exception while trying to run '{executablePath} {arguments}'. Message: {ex.Message}";
            LoggerUtilities.Error(ex, message);
            errorDataReceived?.Invoke(message); // Send error to callback if available
            exitCode = -1;
        }

        return exitCode;
    }
}