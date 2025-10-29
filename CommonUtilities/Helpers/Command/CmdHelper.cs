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

namespace CommonUtilities.Helpers.Command;

/// <summary>
///     Minimal utility for executing external processes using system PATH.
/// </summary>
public static class CmdHelper
{
    /// <summary>
    ///     Runs a process and captures its standard output, standard error, and exit code.
    /// </summary>
    /// <param name="command">The command to run (e.g., "npm", "php").</param>
    /// <param name="arguments">Arguments to pass to the command.</param>
    /// <param name="workingDirectory">Optional working directory.</param>
    /// <returns>Tuple of (output, error, exitCode).</returns>
    public static (string output, string error, int exitCode) RunProcessAndCaptureOutput(string command,
        string arguments, string? workingDirectory = null)
    {
        try
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                return (output, error, process.ExitCode);
            }
        }
        catch (Exception ex)
        {
            return ("", ex.Message, -1);
        }
    }

    /// <summary>
    ///     Runs a process and streams its output and error in real time.
    /// </summary>
    /// <param name="command">The command to run.</param>
    /// <param name="arguments">Arguments to pass to the command.</param>
    /// <param name="workingDirectory">Optional working directory.</param>
    /// <param name="onOutput">Callback for each output line.</param>
    /// <param name="onError">Callback for each error line.</param>
    /// <returns>Exit code of the process.</returns>
    public static int RunProcessWithStreaming(string command, string arguments, string? workingDirectory = null,
        Action<string>? onOutput = null, Action<string>? onError = null)
    {
        try
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                if (onOutput != null)
                    process.OutputDataReceived += (s, e) =>
                    {
                        if (e.Data != null) onOutput(e.Data);
                    };
                if (onError != null)
                    process.ErrorDataReceived += (s, e) =>
                    {
                        if (e.Data != null) onError(e.Data);
                    };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                return process.ExitCode;
            }
        }
        catch (Exception ex)
        {
            onError?.Invoke(ex.Message);
            return -1;
        }
    }

    /// <summary>
    ///     Runs a process using cmd.exe and captures its output, error, and exit code.
    /// </summary>
    public static (string output, string error, int exitCode) RunProcessWithCmd(string command, string args,
        string? workingDirectory = null)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"{command} {args}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory,
                CreateNoWindow = true
            };
            using var process = new Process { StartInfo = psi };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            int exitCode = process.ExitCode;
            return (output, error, exitCode);
        }
        catch (Exception ex)
        {
            return ("", ex.Message, -1);
        }
    }

    /// <summary>
    ///     Runs a process using cmd.exe and streams its output and error in real time.
    /// </summary>
    public static int RunProcessWithCmdStreaming(string command, string args, string? workingDirectory = null,
        Action<string>? onOutput = null, Action<string>? onError = null)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"{command} {args}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory,
                CreateNoWindow = true
            };
            using var process = new Process { StartInfo = psi };
            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null) onOutput?.Invoke(e.Data);
            };
            process.ErrorDataReceived += (s, e) =>
            {
                if (e.Data != null) onError?.Invoke(e.Data);
            };
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            return process.ExitCode;
        }
        catch (Exception ex)
        {
            onError?.Invoke(ex.Message);
            return -1;
        }
    }

    /// <summary>
    ///     Runs a process in a new cmd.exe window (non-blocking, visible window).
    /// </summary>
    public static void RunProcessInNewCmdWindow(string command, string args, string? workingDirectory = null)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/k \"{command} {args}\"",
                UseShellExecute = true,
                WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory,
                CreateNoWindow = false
            };
            Process.Start(psi);
        }
        catch (Exception)
        {
            // Optionally log or handle error
        }
    }
}