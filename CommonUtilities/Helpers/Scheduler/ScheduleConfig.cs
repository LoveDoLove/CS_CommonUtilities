namespace CommonUtilities.Helpers.Scheduler;

public class ScheduleConfig<T> : IScheduleConfig<T>
{
    public string CronExpression { get; set; } = string.Empty;

    public TimeZoneInfo TimeZoneInfo { get; set; } = TimeZoneInfo.Local;
}