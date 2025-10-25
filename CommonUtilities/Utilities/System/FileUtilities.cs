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

using Microsoft.AspNetCore.Http;
using Serilog;

namespace CommonUtilities.Utilities.System;

/// <summary>
///     Provides utility methods for file system operations.
/// </summary>
public static class FileUtilities
{
    /// <summary>
    ///     Deletes a folder and all its contents recursively.
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
    ///     Writes content to a file, overwriting the file if it already exists.
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

    /// <summary>
    ///     Saves the uploaded ebook file to the specified logical path using IWebHostEnvironment.
    /// </summary>
    /// <param name="file">The uploaded form file.</param>
    /// <param name="logicalPath">The logical path under web root (e.g., 'ebooks').</param>
    /// <param name="fileName">The target filename.</param>
    public static string SaveFile(IFormFile file, string logicalPath, string fileName)
    {
        EnsureDirectory(logicalPath);
        string filePath = Path.Combine(logicalPath, fileName);
        using FileStream stream = new(filePath, FileMode.Create);
        file.CopyTo(stream);
        return filePath;
    }

    /// <summary>
    ///     Deletes a file from the specified logical path using IWebHostEnvironment.
    /// </summary>
    /// <param name="logicalPath">The logical path under web root (e.g., 'ebooks').</param>
    /// <param name="fileName">The filename to delete.</param>
    public static void DeleteFile(string logicalPath, string fileName)
    {
        string filePath = Path.Combine(logicalPath, fileName);
        if (File.Exists(filePath)) File.Delete(filePath);
    }

    /// <summary>
    ///     Ensures the target directory exists, creating it if necessary.
    ///     Internal use only.
    /// </summary>
    /// <param name="directory">The directory path to check/create.</param>
    public static void EnsureDirectory(string directory)
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
    }
}