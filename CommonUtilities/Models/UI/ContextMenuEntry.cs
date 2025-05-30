using CommonUtilities.Models.Enums;

namespace CommonUtilities.Models.UI;

/// <summary>
///     Represents an entry in the context menu.
/// </summary>
public class ContextMenuEntry
{
    /// <summary>
    ///     Gets or sets the unique identifier for the context menu entry.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///     Gets or sets the text displayed in the context menu.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    ///     Gets or sets the executable or command to run.
    /// </summary>
    public string Command { get; set; }

    /// <summary>
    ///     Gets or sets the arguments for the command.
    /// </summary>
    public string Arguments { get; set; }

    /// <summary>
    ///     Gets or sets the optional path to an icon for the context menu entry.
    /// </summary>
    public string? IconPath { get; set; }

    /// <summary>
    ///     Gets or sets the target type for the context menu entry (e.g., File, Folder).
    /// </summary>
    public TargetType TargetType { get; set; }

    /// <summary>
    ///     Gets or sets the scope of the context menu entry (e.g., User, System).
    /// </summary>
    public MenuScope Scope { get; set; }
}