using Serilog;

namespace CommonUtilities.Utilities;

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