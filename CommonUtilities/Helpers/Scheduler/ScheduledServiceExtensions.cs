// MIT License
// 
// Copyright (c) 2025 LoveDoLove
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Microsoft.Extensions.DependencyInjection;

namespace CommonUtilities.Helpers.Scheduler;

/// <summary>
///     Provides extension methods for registering scheduled (cron) jobs as hosted services in the dependency injection
///     container.
/// </summary>
public static class ScheduledServiceExtensions
{
    /// <summary>
    ///     Registers a cron job of type <typeparamref name="T" /> as a hosted service with the specified schedule
    ///     configuration.
    /// </summary>
    /// <typeparam name="T">The type of the cron job, which must inherit from <see cref="CronJobHelper" />.</typeparam>
    /// <param name="services">The service collection to add the cron job to.</param>
    /// <param name="options">An action to configure the schedule for the cron job.</param>
    /// <returns>The updated service collection.</returns>
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