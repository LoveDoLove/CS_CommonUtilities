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

namespace CommonUtilities.Utilities.System;

/// <summary>
///     Provides system-related utility functions.
/// </summary>
public static class SystemUtilities
{
    /// <summary>
    ///     Checks if sufficient disk space is available in the drive of the current directory.
    /// </summary>
    /// <returns>True if at least 1GB of free space is available, false otherwise.</returns>
    public static bool CheckDiskSpace()
    {
        try
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string? driveLetter = Path.GetPathRoot(currentDirectory);

            if (string.IsNullOrEmpty(driveLetter))
            {
                Log.Error("Could not determine the drive letter from path: {Path}", currentDirectory);
                return false;
            }

            DriveInfo drive = new DriveInfo(driveLetter);

            // Consider healthy if at least 1GB free
            const long minimumFreeSpace = 1024 * 1024 * 1024; // 1GB
            bool hasSpace = drive.AvailableFreeSpace > minimumFreeSpace;

            if (!hasSpace)
                Log.Warning("Low disk space: {AvailableMB}MB free on drive {DriveName}",
                    drive.AvailableFreeSpace / (1024 * 1024), drive.Name);

            return hasSpace;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error checking disk space");
            return false;
        }
    }
}