namespace CommonUtilities.Utilities.System;

/// <summary>
///     Provides utility methods for working with timestamps.
/// </summary>
public static class TimestampUtilities
{
    /// <summary>
    ///     Gets the current Coordinated Universal Time (UTC) as a Unix epoch timestamp (seconds since 1970-01-01T00:00:00Z).
    /// </summary>
    /// <returns>The current UTC epoch time in seconds.</returns>
    public static long GetEpochTime()
    {
        return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }
}