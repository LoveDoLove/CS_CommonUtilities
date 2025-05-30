using CommonUtilities.Models;

namespace CommonUtilities.Interfaces;

/// <summary>
///     Defines the contract for managing context menu entries.
/// </summary>
public interface IContextMenuManager
{
    /// <summary>
    ///     Adds a new entry to the context menu.
    /// </summary>
    /// <param name="entry">The context menu entry to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddEntryAsync(ContextMenuEntry entry);

    /// <summary>
    ///     Removes an entry from the context menu.
    /// </summary>
    /// <param name="entryId">The unique identifier of the context menu entry to remove.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveEntryAsync(string entryId);

    /// <summary>
    ///     Checks if a context menu entry exists.
    /// </summary>
    /// <param name="entryId">The unique identifier of the context menu entry to check.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains true if the entry exists;
    ///     otherwise, false.
    /// </returns>
    Task<bool> CheckEntryExistsAsync(string entryId);

    /// <summary>
    ///     Retrieves all context menu entries.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a collection of all context menu
    ///     entries.
    /// </returns>
    Task<IEnumerable<ContextMenuEntry>> GetEntriesAsync();
}