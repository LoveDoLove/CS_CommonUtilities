using System.Text;
using CommonUtilities.Interfaces;
using CommonUtilities.Models;
using CommonUtilities.Models.Enums;

namespace CommonUtilities.Services.OSIntegration.Linux;

/// <summary>
///     Manages context menu entries for Linux desktop environments.
///     Creates .desktop files for file manager actions.
/// </summary>
public class LinuxContextMenuManager : IContextMenuManager
{
    // Base paths for .desktop files
    // User-specific actions (e.g., Nautilus, Caja, Nemo)
    private const string UserActionsPath1 = ".local/share/file-manager/actions/";

    // User-specific actions (e.g., KDE Dolphin)
    private const string UserKdeServiceMenusPath = ".local/share/kservices5/ServiceMenus/";

    // System-wide actions (requires root privileges to write here)
    private const string SystemActionsPath = "/usr/share/file-manager/actions/";

    // System-wide KDE actions
    private const string SystemKdeServiceMenusPath = "/usr/share/kservices5/ServiceMenus/";

    // TODO: Inject or configure a logger
    // private readonly ILogger _logger;

    // public LinuxContextMenuManager(ILogger logger)
    // {
    //     _logger = logger;
    // }

    /// <summary>
    ///     Adds a new entry to the context menu by creating a .desktop file.
    /// </summary>
    /// <param name="entry">The context menu entry to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task AddEntryAsync(ContextMenuEntry entry)
    {
        if (entry == null)
            // _logger.LogWarning("Attempted to add a null context menu entry.");
            throw new ArgumentNullException(nameof(entry));
        if (string.IsNullOrWhiteSpace(entry.Id))
            // _logger.LogWarning("Attempted to add a context menu entry with a null or empty ID.");
            throw new ArgumentException("Entry ID cannot be null or empty.", nameof(entry.Id));

        string basePath = GetBasePath(entry.Scope);
        // For simplicity, we'll primarily target the generic file-manager/actions path.
        // For KDE, a different path or even a different file format might be more appropriate.
        // We'll use UserActionsPath1 for user scope. A more robust solution might detect the DE.
        string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), basePath);

        if (entry.Scope == MenuScope.System) directoryPath = basePath; // System paths are absolute
        // _logger.LogInformation($"System scope selected. Path: {directoryPath}. This typically requires root privileges.");
        try
        {
            Directory.CreateDirectory(directoryPath); // Ensure the directory exists

            string desktopFilePath = Path.Combine(directoryPath, $"{entry.Id}.desktop");
            string desktopFileContent = GenerateDesktopFileContent(entry);

            await File.WriteAllTextAsync(desktopFilePath, desktopFileContent, Encoding.UTF8);
            // _logger.LogInformation($"Successfully added context menu entry: {entry.Id} at {desktopFilePath}");
        }
        catch (UnauthorizedAccessException ex)
        {
            // _logger.LogError(ex, $"Permission denied when trying to add context menu entry: {entry.Id} to {directoryPath}. For system scope, ensure you have root privileges.");
            throw new UnauthorizedAccessException(
                $"Permission denied for path {directoryPath}. Ensure correct permissions.", ex);
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, $"Failed to add context menu entry: {entry.Id}");
            throw new IOException($"Failed to add context menu entry '{entry.Id}'.", ex);
        }
    }

    /// <summary>
    ///     Removes an entry from the context menu by deleting its .desktop file.
    /// </summary>
    /// <param name="entryId">The unique identifier of the context menu entry to remove (filename without .desktop extension).</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task RemoveEntryAsync(string entryId)
    {
        if (string.IsNullOrWhiteSpace(entryId))
            // _logger.LogWarning("Attempted to remove a context menu entry with a null or empty ID.");
            throw new ArgumentException("Entry ID cannot be null or empty.", nameof(entryId));

        // Try to remove from user paths first, then system paths.
        // This is a simplification; a robust solution might store where an entry was created.
        var potentialPaths = new List<string>
        {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), UserActionsPath1,
                $"{entryId}.desktop"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), UserKdeServiceMenusPath,
                $"{entryId}.desktop"),
            Path.Combine(SystemActionsPath, $"{entryId}.desktop"),
            Path.Combine(SystemKdeServiceMenusPath, $"{entryId}.desktop")
        };

        bool removed = false;
        foreach (var path in potentialPaths)
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    // _logger.LogInformation($"Successfully removed context menu entry: {entryId} from {path}");
                    removed = true;
                    break; // Found and removed
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // _logger.LogWarning(ex, $"Permission denied when trying to remove context menu entry: {entryId} from {path}.");
                // Continue trying other paths, but rethrow if it's the only place it existed and failed.
                // For simplicity, we just log and continue. If it's critical, this logic needs refinement.
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, $"Failed to remove context menu entry: {entryId} from {path}");
                // Potentially rethrow or handle more gracefully
                throw new IOException($"Error removing context menu entry '{entryId}' from '{path}'.", ex);
            }

        if (!removed)
        {
            // _logger.LogInformation($"Context menu entry not found for removal: {entryId}");
            // Consider if this should be an error or just a silent failure if not found.
            // For now, if it's not found in any standard location, we assume it's already gone or never existed.
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Checks if a context menu entry exists by looking for its .desktop file.
    /// </summary>
    /// <param name="entryId">The unique identifier of the context menu entry to check (filename without .desktop extension).</param>
    /// <returns>True if the entry exists; otherwise, false.</returns>
    public Task<bool> CheckEntryExistsAsync(string entryId)
    {
        if (string.IsNullOrWhiteSpace(entryId))
            // _logger.LogWarning("Attempted to check a context menu entry with a null or empty ID.");
            return Task.FromResult(false); // Or throw ArgumentException

        var potentialPaths = new List<string>
        {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), UserActionsPath1,
                $"{entryId}.desktop"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), UserKdeServiceMenusPath,
                $"{entryId}.desktop"),
            Path.Combine(SystemActionsPath, $"{entryId}.desktop"),
            Path.Combine(SystemKdeServiceMenusPath, $"{entryId}.desktop")
        };

        foreach (var path in potentialPaths)
            if (File.Exists(path))
                // _logger.LogInformation($"Context menu entry found: {entryId} at {path}");
                return Task.FromResult(true);

        // _logger.LogInformation($"Context menu entry not found: {entryId}");
        return Task.FromResult(false);
    }

    /// <summary>
    ///     Retrieves all context menu entries managed by this application.
    ///     NOTE: This implementation is a placeholder as per requirements.
    ///     A full implementation would require scanning directories and parsing .desktop files,
    ///     potentially with a specific marker to identify application-managed entries.
    /// </summary>
    /// <returns>An empty list of context menu entries.</returns>
    public Task<IEnumerable<ContextMenuEntry>> GetEntriesAsync()
    {
        // _logger.LogInformation("GetEntriesAsync called, returning empty list as per current design.");
        // A full implementation would scan relevant directories (UserActionsPath1, UserKdeServiceMenusPath, etc.)
        // and parse .desktop files, possibly looking for a custom field to identify entries managed by this app.
        return Task.FromResult(Enumerable.Empty<ContextMenuEntry>());
    }

    /// <summary>
    ///     Gets the base directory path for context menu entries based on scope.
    /// </summary>
    private string GetBasePath(MenuScope scope)
    {
        // Prioritize generic file-manager/actions path.
        // A more sophisticated approach might detect the desktop environment.
        switch (scope)
        {
            case MenuScope.User:
                // Could also check for KDE and return UserKdeServiceMenusPath
                return UserActionsPath1;
            case MenuScope.System:
                // Could also check for KDE and return SystemKdeServiceMenusPath
                return SystemActionsPath;
            default:
                // _logger.LogWarning($"Unknown MenuScope: {scope}. Defaulting to user path.");
                return UserActionsPath1; // Default to user-specific path
        }
    }

    /// <summary>
    ///     Generates the content for a .desktop file based on the ContextMenuEntry.
    /// </summary>
    private string GenerateDesktopFileContent(ContextMenuEntry entry)
    {
        var sb = new StringBuilder();
        sb.AppendLine("[Desktop Entry]");
        sb.AppendLine("Type=Action");
        sb.AppendLine($"Name={entry.Text}");

        if (!string.IsNullOrWhiteSpace(entry.IconPath)) sb.AppendLine($"Icon={entry.IconPath}");

        string profileString;
        string execCommand = $"{entry.Command}";
        if (!string.IsNullOrWhiteSpace(entry.Arguments)) execCommand += $" {entry.Arguments}";

        // Determine profile string and exec parameter based on TargetType
        // %F for list of files/dirs, %f for single file, %U for list of URLs, %u for single URL
        // %d for single directory (if action is on background), %D for list of directories
        // For simplicity, using %F as a general case for files/folders.
        // Background might need %d or the current directory.
        // The prompt suggests "%F {entry.Arguments}" for Exec.
        // Let's refine the profile string and exec based on TargetType.

        string mimeTypeCondition = ""; // For MimeType based activation

        switch (entry.TargetType)
        {
            case TargetType.File:
                profileString = "nodirs"; // Appears for files, not for directories
                execCommand = $"{execCommand} %F"; // %F for multiple files
                // mimeTypeCondition = "MimeType=text/plain;application/pdf;"; // Example
                break;
            case TargetType.Folder:
                profileString = "dirs"; // Appears for directories
                execCommand = $"{execCommand} %F"; // %F for multiple directories (paths)
                break;
            case TargetType.Drive: // Treat drives similar to folders for .desktop actions
                profileString = "dirs";
                execCommand = $"{execCommand} %F";
                break;
            case TargetType.Background:
                // For background, the action is on the directory itself.
                // 'dirs' profile makes it appear on directories.
                // The command might need the current directory path, often %d.
                profileString = "dirs"; // To make it appear when a directory is selected or for directory background.
                // Some file managers might use a different mechanism or profile for background.
                // For Nautilus, actions in `~/.local/share/nautilus/scripts` are simpler for background.
                // Here, we assume the action is associated with the directory itself.
                execCommand = $"{execCommand} %d"; // %d for the current directory when action is on background
                break;
            default:
                profileString = "allfiles"; // Fallback: appears for all files and directories
                execCommand = $"{execCommand} %F";
                break;
        }

        sb.AppendLine($"Profiles={profileString};");
        sb.AppendLine();
        sb.AppendLine($"[X-Action-Profile {profileString}]");
        sb.AppendLine($"Name={entry.Text}"); // Name again under profile
        sb.AppendLine($"Exec={execCommand}");
        // If MimeTypes are preferred over profiles for some file managers or more specific targeting:
        // if (!string.IsNullOrWhiteSpace(mimeTypeCondition))
        // {
        //     sb.AppendLine(mimeTypeCondition);
        // }
        // else
        // {
        //    sb.AppendLine($"Profiles={profileString};");
        // }


        return sb.ToString();
    }
}