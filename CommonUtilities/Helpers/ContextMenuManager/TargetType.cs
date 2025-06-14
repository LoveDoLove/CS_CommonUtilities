namespace CommonUtilities.Helpers.ContextMenuManager;

/// <summary>
///     Specifies the target type for a context menu entry.
/// </summary>
public enum TargetType
{
    /// <summary>
    ///     The context menu entry targets a file.
    /// </summary>
    File,

    /// <summary>
    ///     The context menu entry targets a folder.
    /// </summary>
    Folder,

    /// <summary>
    ///     The context menu entry targets a drive.
    /// </summary>
    Drive,

    /// <summary>
    ///     The context menu entry targets the background of a folder.
    /// </summary>
    Background
}