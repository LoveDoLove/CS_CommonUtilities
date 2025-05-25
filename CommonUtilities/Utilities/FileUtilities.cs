using Serilog;

namespace CommonUtilities.Utilities;

/// <summary>
/// Provides utility methods for file system operations.
/// </summary>
public static class FileUtilities
{
    /// <summary>
    /// Deletes a folder and all its contents recursively.
    /// </summary>
    /// <param name="directoryPath">The path to the directory to delete.</param>
    /// <returns>True if the directory was successfully deleted, false otherwise.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown if the directory does not exist.</exception>
    /// <exception cref="IOException">Thrown if an I/O error occurs.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission.</exception>
    /// <exception cref="Exception">Thrown for other errors during deletion.</exception>
    public static bool DeleteFolder(string directoryPath)
    {
        try
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

            foreach (string filePath in Directory.GetFiles(directoryPath))
                File.SetAttributes(filePath, FileAttributes.Normal);

            foreach (string subDirectoryPath in Directory.GetDirectories(directoryPath)) DeleteFolder(subDirectoryPath);

            File.SetAttributes(directoryPath, FileAttributes.Normal);

            Directory.Delete(directoryPath, true);

            Log.Information($"Directory deleted: {directoryPath}");
            return true;
        }
        catch (DirectoryNotFoundException ex) // Specific exception
        {
            Log.Error(ex, "Directory not found during deletion: {DirectoryPath}", directoryPath);
            throw; // Rethrow original exception
        }
        catch (IOException ex) // Specific exception for I/O errors
        {
            Log.Error(ex, "IO error deleting directory: {DirectoryPath}", directoryPath);
            throw new IOException($"Error deleting directory: {directoryPath}. {ex.Message}", ex);
        }
        catch (UnauthorizedAccessException ex) // Specific exception for permission issues
        {
            Log.Error(ex, "Unauthorized access deleting directory: {DirectoryPath}", directoryPath);
            throw new UnauthorizedAccessException(
                $"Error deleting directory due to permissions: {directoryPath}. {ex.Message}", ex);
        }
        catch (Exception ex) // General fallback
        {
            Log.Error(ex, "Generic error deleting directory: {DirectoryPath}", directoryPath);
            throw new Exception($"Error deleting directory: {directoryPath}", ex);
        }
    }

    /// <summary>
    /// Writes content to a file, overwriting the file if it already exists.
    /// </summary>
    /// <param name="path">The path to the file to write.</param>
    /// <param name="content">The string content to write to the file.</param>
    /// <returns>True if the file was successfully written, false otherwise.</returns>
    /// <exception cref="IOException">Thrown if an I/O error occurs.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have the required permission.</exception>
    /// <exception cref="Exception">Thrown for other errors during writing.</exception>
    public static bool WriteFile(string path, string content)
    {
        try
        {
            File.WriteAllText(path, content);
            Log.Information($"Data stored in {path}");
            return true;
        }
        catch (IOException ex) // Specific exception for I/O errors
        {
            Log.Error(ex, "IO error writing file: {Path}", path);
            throw new IOException($"Error writing file: {path}. {ex.Message}", ex);
        }
        catch (UnauthorizedAccessException ex) // Specific exception for permission issues
        {
            Log.Error(ex, "Unauthorized access writing file: {Path}", path);
            throw new UnauthorizedAccessException($"Error writing file due to permissions: {path}. {ex.Message}", ex);
        }
        catch (Exception ex) // General fallback
        {
            Log.Error(ex, "Generic error writing file: {Path}", path);
            throw new Exception($"Error writing file: {path}", ex);
        }
    }
}