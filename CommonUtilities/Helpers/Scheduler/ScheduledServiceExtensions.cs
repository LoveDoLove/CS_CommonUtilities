using Microsoft.Extensions.DependencyInjection;

namespace CommonUtilities.Helpers.Scheduler;

public static class ScheduledServiceExtensions
{
    public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options)
        where T : CronJobHelper // Constraint: T must be a type that inherits from CronJobService
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");

        // Create a new configuration instance and apply the provided options
        var config = new ScheduleConfig<T>();
        options.Invoke(config);

        // Validate that a CRON expression has been provided
        if (string.IsNullOrWhiteSpace(config.CronExpression))
            throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression),
                @"Empty Cron Expression is not allowed.");

        // Register the configuration as a singleton, so it can be injected into the CronJobService
        services.AddSingleton<IScheduleConfig<T>>(config);

        // Register the CronJobService itself as a hosted service
        services.AddHostedService<T>();

        return services;
    }
}