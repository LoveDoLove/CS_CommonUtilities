// Copyright 2025 LoveDoLove
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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