using Microsoft.Extensions.DependencyInjection;

namespace CommonUtilities.Utilities.Scheduler;

/// <summary>
///     Provides extension methods for <see cref="IServiceCollection" /> to facilitate the registration of CRON job
///     services.
/// </summary>
public static class ScheduledServiceExtensions
{
    /// <summary>
    ///     Adds a CRON job service of type <typeparamref name="T" /> to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <typeparam name="T">The type of the CRON job service to add. Must be a subclass of <see cref="CronJobService" />.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
    /// <param name="options">An <see cref="Action{T}" /> to configure the <see cref="IScheduleConfig{T}" /> for the CRON job.</param>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="options" /> is null, or if the <see cref="IScheduleConfig{T}.CronExpression" /> within
    ///     the options is null, empty, or whitespace.
    /// </exception>
    public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options)
        where T : CronJobService // Constraint: T must be a type that inherits from CronJobService
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