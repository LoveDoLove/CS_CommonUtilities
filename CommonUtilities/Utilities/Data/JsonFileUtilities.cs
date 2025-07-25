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

using System.Text.Json;
using CommonUtilities.Utilities.Security;
using CommonUtilities.Utilities.System;

namespace CommonUtilities.Utilities.Data;

/// <summary>
///     Provides utility methods for reading from and writing to JSON files, with optional AES encryption and decryption.
/// </summary>
public static class JsonFileUtilities
{
    private const string SupportedFileType = ".json";

    /// <summary>
    ///     Reads the content of a JSON file.
    ///     Optionally decrypts the file content if an AES key and IV are provided.
    /// </summary>
    /// <param name="filePath">The path to the JSON file.</param>
    /// <param name="aesKey">Optional AES key for decryption. If provided, <paramref name="iv" /> must also be provided.</param>
    /// <param name="iv">
    ///     Optional AES initialization vector for decryption. If provided, <paramref name="aesKey" /> must also
    ///     be provided.
    /// </param>
    /// <returns>The content of the JSON file as a string. If decryption was requested, returns the decrypted content.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
    /// <exception cref="ArgumentException">Thrown if the file type is not '.json'.</exception>
    /// <exception cref="JsonException">
    ///     Thrown if the file content is empty and decryption is attempted, or if other
    ///     JSON-specific errors occur.
    /// </exception>
    /// <exception cref="IOException">Thrown if an error occurs during file reading or decryption.</exception>
    public static string ReadJsonFile(string filePath, string? aesKey = null, string? iv = null)
    {
        try
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File not found: {filePath}", filePath);

            string fileType = Path.GetExtension(filePath);
            if (!string.Equals(fileType, SupportedFileType, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"Invalid file type: {fileType}. Only {SupportedFileType} is supported.",
                    nameof(filePath));

            string fileData = File.ReadAllText(filePath);

            // Decryption is requested if both aesKey and iv are provided
            if (aesKey != null && iv != null)
            {
                if (string.IsNullOrEmpty(fileData))
                    // Cannot decrypt an empty file/content
                    throw new JsonException($"File content is empty and cannot be decrypted: {filePath}");
                return AesUtilities.Aes256CbcDecrypt(fileData, aesKey, iv);
            }

            // No decryption requested, return fileData as is (could be empty)
            return fileData;
        }
        catch
            (JsonException) // Rethrow JsonException as it's specific to JSON processing or our custom empty file check
        {
            throw;
        }
        catch (Exception e)
        {
            // Wrap other exceptions (e.g., from File.ReadAllText or AesUtilities) in IOException for consistent error handling for file operations.
            throw new IOException($"Error reading or decrypting file: {filePath}. Details: {e.Message}", e);
        }
    }

    /// <summary>
    ///     Saves the provided data object as a JSON file.
    ///     Optionally encrypts the file content if an AES key and IV are provided.
    /// </summary>
    /// <param name="filePath">The path where the JSON file will be saved.</param>
    /// <param name="data">The object to serialize and save.</param>
    /// <param name="aesKey">Optional AES key for encryption. If provided, <paramref name="iv" /> must also be provided.</param>
    /// <param name="iv">
    ///     Optional AES initialization vector for encryption. If provided, <paramref name="aesKey" /> must also
    ///     be provided.
    /// </param>
    /// <returns><c>true</c> if the file was saved successfully.</returns>
    /// <exception cref="IOException">Thrown if an error occurs during serialization, encryption, or saving the file.</exception>
    public static bool SaveFileAsJson(string filePath, object data, string? aesKey = null, string? iv = null)
    {
        try
        {
            string serializedData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

            // Encryption is performed if both aesKey and iv are provided
            if (aesKey != null && iv != null)
            {
                // Use the provided aesKey and iv for encryption
                string encryptedData = AesUtilities.Aes256CbcEncrypt(serializedData, aesKey, iv);
                FileUtilities.WriteFile(filePath, encryptedData);
            }
            else
            {
                // No encryption requested, save serialized data directly
                FileUtilities.WriteFile(filePath, serializedData);
            }

            return true;
        }
        catch (Exception e)
        {
            // Wrap exceptions (e.g., from JsonSerializer.Serialize, AesUtilities, or FileUtilities.WriteFile)
            // in IOException for consistent error handling for file operations.
            throw new IOException($"Error serializing, encrypting, or saving file: {filePath}. Details: {e.Message}",
                e);
        }
    }
}