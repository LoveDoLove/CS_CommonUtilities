using System.Security;
using CommonUtilities.Interfaces;
using CommonUtilities.Models.Enums;
using Microsoft.Win32;

namespace CommonUtilities.Services.OSIntegration.Windows;

/// <summary>
///     Manages context menu entries for Windows.
/// </summary>
public class WindowsContextMenuManager : IContextMenuManager
{
    private const string
        AppRegistrySubKey = @"Software\EasyKit\ContextMenuEntries"; // A place to store our app-specific entry details

    /// <summary>
    ///     Adds a new entry to the Windows context menu.
    /// </summary>
    /// <param name="entry">The context menu entry to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddEntryAsync(ContextMenuEntry entry)
    {
        // Placeholder for logging: LoggerUtilities.Log($"Adding context menu entry: {entry.Id}");
        RegistryKey? baseKey = GetBaseKey(entry.Scope);
        if (baseKey == null)
            // Placeholder for logging: LoggerUtilities.Log($"Unsupported scope: {entry.Scope} for entry: {entry.Id}", LogLevel.Error);
            return Task.CompletedTask; // Or throw an exception

        string targetPath = GetRegistryPathForTarget(entry.TargetType);
        if (string.IsNullOrEmpty(targetPath))
            // Placeholder for logging: LoggerUtilities.Log($"Unsupported target type: {entry.TargetType} for entry: {entry.Id}", LogLevel.Error);
            return Task.CompletedTask; // Or throw an exception

        // Use entry.Id for the key name to ensure uniqueness and easy removal
        string entryKeyName = entry.Id;
        if (string.IsNullOrWhiteSpace(entryKeyName))
            // Placeholder for logging: LoggerUtilities.Log($"Entry Id cannot be empty for entry: {entry.Text}", LogLevel.Error);
            throw new ArgumentException("Entry Id cannot be empty.", nameof(entry.Id));

        using (RegistryKey? shellKey = baseKey.CreateSubKey(Path.Combine(targetPath, entryKeyName), true))
        {
            if (shellKey == null)
                // Placeholder for logging: LoggerUtilities.Log($"Failed to create shell key for entry: {entry.Id}", LogLevel.Error);
                return Task.CompletedTask;
            shellKey.SetValue("", entry.Text); // Set the display text
            if (!string.IsNullOrWhiteSpace(entry.IconPath)) shellKey.SetValue("Icon", entry.IconPath);

            using (RegistryKey? commandKey = shellKey.CreateSubKey("command", true))
            {
                if (commandKey == null)
                    // Placeholder for logging: LoggerUtilities.Log($"Failed to create command key for entry: {entry.Id}", LogLevel.Error);
                    return Task.CompletedTask;
                string commandValue = $"\"{entry.Command}\" \"%1\"";
                if (!string.IsNullOrWhiteSpace(entry.Arguments)) commandValue += $" {entry.Arguments}";
                commandKey.SetValue("", commandValue);
            }
        }

        // Store metadata for GetEntriesAsync and easier management
        StoreEntryMetadata(entry);

        // Placeholder for logging: LoggerUtilities.Log($"Successfully added context menu entry: {entry.Id}");

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Removes an entry from the Windows context menu.
    /// </summary>
    /// <param name="entryId">The unique identifier of the context menu entry to remove.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task RemoveEntryAsync(string entryId)
    {
        // Placeholder for logging: LoggerUtilities.Log($"Removing context menu entry: {entryId}");
        try
        {
            // Retrieve metadata to know where the entry was created
            ContextMenuEntry? entryMeta = GetStoredEntryMetadata(entryId);
            if (entryMeta == null)
                // Placeholder for logging: LoggerUtilities.Log($"No metadata found for entryId: {entryId}. Cannot determine scope or target type for removal.", LogLevel.Warning);
                // Attempt removal from common paths if metadata is missing (less reliable)
                // For now, we'll assume metadata exists or removal fails.
                // A more robust solution would iterate through all possible TargetType/Scope combinations.
                return Task.CompletedTask;

            RegistryKey? baseKey = GetBaseKey(entryMeta.Scope);
            if (baseKey == null)
                // Placeholder for logging: LoggerUtilities.Log($"Unsupported scope: {entryMeta.Scope} for entry: {entryId}", LogLevel.Error);
                return Task.CompletedTask;

            string targetPath = GetRegistryPathForTarget(entryMeta.TargetType);
            if (string.IsNullOrEmpty(targetPath))
                // Placeholder for logging: LoggerUtilities.Log($"Unsupported target type: {entryMeta.TargetType} for entry: {entryId}", LogLevel.Error);
                return Task.CompletedTask;

            baseKey.DeleteSubKeyTree(Path.Combine(targetPath, entryId), false); // false: do not throw if not found

            RemoveStoredEntryMetadata(entryId);
            // Placeholder for logging: LoggerUtilities.Log($"Successfully removed context menu entry: {entryId}");
        }
        catch (ArgumentException
               ex) // Can be thrown by DeleteSubKeyTree if key doesn't exist and throwOnMissing is true (though we use false)
        {
            // Placeholder for logging: LoggerUtilities.Log($"ArgumentException (key likely not found) while removing entry {entryId}: {ex.Message}", LogLevel.Warning);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Checks if a context menu entry exists in the Windows Registry.
    /// </summary>
    /// <param name="entryId">The unique identifier of the context menu entry to check.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains true if the entry exists;
    ///     otherwise, false.
    /// </returns>
    public Task<bool> CheckEntryExistsAsync(string entryId)
    {
        // Placeholder for logging: LoggerUtilities.Log($"Checking if context menu entry exists: {entryId}");
        try
        {
            ContextMenuEntry? entryMeta = GetStoredEntryMetadata(entryId);
            if (entryMeta == null)
                // Placeholder for logging: LoggerUtilities.Log($"No metadata found for entryId: {entryId}. Cannot determine scope or target type for check.", LogLevel.Warning);
                return Task.FromResult(false); // If we don't know where it should be, assume it doesn't exist.

            RegistryKey? baseKey = GetBaseKey(entryMeta.Scope);
            if (baseKey == null) return Task.FromResult(false);

            string targetPath = GetRegistryPathForTarget(entryMeta.TargetType);
            if (string.IsNullOrEmpty(targetPath)) return Task.FromResult(false);

            using (RegistryKey? entryKey = baseKey.OpenSubKey(Path.Combine(targetPath, entryId)))
            {
                return Task.FromResult(entryKey != null);
            }
        }
        catch (Exception ex)
        {
            // Placeholder for logging: LoggerUtilities.Log($"Exception while checking entry {entryId}: {ex.Message}", LogLevel.Error);
            return Task.FromResult(false); // On error, assume it doesn't exist or cannot be verified
        }
    }

    /// <summary>
    ///     Retrieves all custom context menu entries managed by this application from the Windows Registry.
    ///     This implementation relies on metadata stored by the application.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a collection of all context menu
    ///     entries.
    /// </returns>
    public Task<IEnumerable<ContextMenuEntry>> GetEntriesAsync()
    {
        // Placeholder for logging: LoggerUtilities.Log("Getting all managed context menu entries.");
        var entries = new List<ContextMenuEntry>();
        try
        {
            // Iterate over both HKEY_CURRENT_USER and HKEY_LOCAL_MACHINE for our app's metadata
            ProcessScopeForGetEntries(Registry.CurrentUser, entries);
            ProcessScopeForGetEntries(Registry.LocalMachine, entries);
        }
        catch (Exception ex)
        {
            // Placeholder for logging: LoggerUtilities.Log($"Exception while getting entries: {ex.Message}", LogLevel.Error);
            // Return what we have, or an empty list if critical error
        }

        return Task.FromResult<IEnumerable<ContextMenuEntry>>(entries);
    }

    private void ProcessScopeForGetEntries(RegistryKey rootKey, List<ContextMenuEntry> entries)
    {
        try
        {
            using (RegistryKey? appMetaKey = rootKey.OpenSubKey(AppRegistrySubKey, false))
            {
                if (appMetaKey != null)
                    foreach (string entryId in appMetaKey.GetSubKeyNames())
                    {
                        ContextMenuEntry? entry = GetStoredEntryMetadata(entryId, rootKey);
                        if (entry != null) entries.Add(entry);
                    }
            }
        }
        catch (SecurityException ex)
        {
            // Placeholder for logging: LoggerUtilities.Log($"SecurityException while accessing metadata in {rootKey.Name}: {ex.Message}", LogLevel.Warning);
        }
        catch (UnauthorizedAccessException ex)
        {
            // Placeholder for logging: LoggerUtilities.Log($"UnauthorizedAccessException while accessing metadata in {rootKey.Name}: {ex.Message}", LogLevel.Warning);
        }
    }


    private RegistryKey? GetBaseKey(MenuScope scope)
    {
        return scope switch
        {
            MenuScope.User => Registry.CurrentUser,
            MenuScope.System => Registry.LocalMachine,
            _ => null
        };
    }

    private string GetRegistryPathForTarget(TargetType targetType)
    {
        return targetType switch
        {
            TargetType.File => @"Software\Classes\*\shell",
            TargetType.Folder => @"Software\Classes\Directory\shell", // Changed from Directory
            TargetType.Background =>
                @"Software\Classes\Directory\Background\shell", // Changed from DirectoryBackground and covers DesktopBackground conceptually
            TargetType.Drive => @"Software\Classes\Drive\shell",
            // Removed AllFilesAndFolders as it's not in the TargetType enum
            // Removed DesktopBackground as it's covered by Background
            _ => string.Empty
        };
    }

    private void StoreEntryMetadata(ContextMenuEntry entry)
    {
        RegistryKey? baseKey = GetBaseKey(entry.Scope);
        if (baseKey == null) return;

        try
        {
            using (RegistryKey? entryMetaKey = baseKey.CreateSubKey(Path.Combine(AppRegistrySubKey, entry.Id), true))
            {
                if (entryMetaKey == null) return;
                entryMetaKey.SetValue("Text", entry.Text);
                entryMetaKey.SetValue("Command", entry.Command);
                entryMetaKey.SetValue("Arguments", entry.Arguments ?? string.Empty);
                entryMetaKey.SetValue("IconPath", entry.IconPath ?? string.Empty);
                entryMetaKey.SetValue("TargetType", entry.TargetType.ToString());
                entryMetaKey.SetValue("Scope", entry.Scope.ToString()); // Store scope as well for completeness
            }
        }
        catch (Exception ex)
        {
            // Placeholder for logging: LoggerUtilities.Log($"Failed to store metadata for {entry.Id}: {ex.Message}", LogLevel.Error);
        }
    }

    private ContextMenuEntry? GetStoredEntryMetadata(string entryId, RegistryKey? specificBaseKey = null)
    {
        RegistryKey? baseKeyToSearchFirst = specificBaseKey;
        RegistryKey? baseKeyToSearchSecond = null;

        if (baseKeyToSearchFirst == null) // If no specific base key, try both
        {
            baseKeyToSearchFirst = Registry.CurrentUser;
            baseKeyToSearchSecond = Registry.LocalMachine;
        }

        ContextMenuEntry? entry = TryGetMetadataFromKey(baseKeyToSearchFirst, entryId);
        if (entry == null && baseKeyToSearchSecond != null)
            entry = TryGetMetadataFromKey(baseKeyToSearchSecond, entryId);
        return entry;
    }

    private ContextMenuEntry? TryGetMetadataFromKey(RegistryKey baseKey, string entryId)
    {
        try
        {
            using (RegistryKey? entryMetaKey = baseKey.OpenSubKey(Path.Combine(AppRegistrySubKey, entryId), false))
            {
                if (entryMetaKey == null) return null;

                return new ContextMenuEntry
                {
                    Id = entryId,
                    Text = entryMetaKey.GetValue("Text") as string ?? string.Empty,
                    Command = entryMetaKey.GetValue("Command") as string ?? string.Empty,
                    Arguments = entryMetaKey.GetValue("Arguments") as string ?? string.Empty,
                    IconPath = entryMetaKey.GetValue("IconPath") as string ?? string.Empty,
                    TargetType = Enum.TryParse<TargetType>(entryMetaKey.GetValue("TargetType") as string, out var tt)
                        ? tt
                        : default,
                    Scope = Enum.TryParse<MenuScope>(entryMetaKey.GetValue("Scope") as string, out var ms)
                        ? ms
                        : default
                };
            }
        }
        catch (Exception ex)
        {
            // Placeholder for logging: LoggerUtilities.Log($"Failed to get metadata for {entryId} from {baseKey.Name}: {ex.Message}", LogLevel.Warning);
            return null;
        }
    }


    private void RemoveStoredEntryMetadata(string entryId)
    {
        // Try removing from both scopes, as we might not know which one it was if GetStoredEntryMetadata failed before this call
        RemoveMetadataFromScope(Registry.CurrentUser, entryId);
        RemoveMetadataFromScope(Registry.LocalMachine, entryId);
    }

    private void RemoveMetadataFromScope(RegistryKey baseKey, string entryId)
    {
        try
        {
            using (RegistryKey? appMetaKey = baseKey.OpenSubKey(AppRegistrySubKey, true)) // Open with write access
            {
                appMetaKey?.DeleteSubKeyTree(entryId, false); // false: do not throw if not found
            }
        }
        catch (Exception ex)
        {
            // Placeholder for logging: LoggerUtilities.Log($"Failed to remove metadata for {entryId} from {baseKey.Name}: {ex.Message}", LogLevel.Warning);
        }
    }
}