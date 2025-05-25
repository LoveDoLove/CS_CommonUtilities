namespace CommonUtilities.Helpers.Scheduler;

/// <summary>
/// Defines the configuration for a scheduled job of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the job being configured. This type parameter allows for specific configurations if needed, though it's not strictly used by the properties in this interface.</typeparam>
public interface IScheduleConfig<T>
{
    /// <summary>
    /// Gets or sets the CRON expression string that defines the schedule for the job.
    /// </summary>
    string CronExpression { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="System.TimeZoneInfo"/> for the CRON schedule.
    /// This determines how the CRON expression is interpreted regarding time zones.
    /// </summary>
    TimeZoneInfo TimeZoneInfo { get; set; }
}

/// <summary>
/// Represents the configuration for a scheduled job of type <typeparamref name="T"/>.
/// Implements the <see cref="IScheduleConfig{T}"/> interface.
/// </summary>
/// <typeparam name="T">The type of the job being configured.</typeparam>
public class ScheduleConfig<T> : IScheduleConfig<T>
{
    /// <summary>
    /// Gets or sets the CRON expression string that defines the schedule for the job.
    /// Defaults to an empty string, which is an invalid CRON expression and should be set.
    /// </summary>
    public string CronExpression { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the <see cref="System.TimeZoneInfo"/> for the CRON schedule.
    /// Defaults to <see cref="System.TimeZoneInfo.Local"/>, the local time zone of the server.
    /// </summary>
    public TimeZoneInfo TimeZoneInfo { get; set; } = TimeZoneInfo.Local;
}