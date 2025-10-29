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

using CommonUtilities.Helpers.Scheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CommonUtilities.Services.Sync;

/// <summary>
///     Base class for scheduled background jobs using cron syntax.
///     Inherit from this class to implement your own periodic sync logic.
///     Register your derived service with AddHostedService in your DI setup.
///     See Context7/ASP.NET Core docs for background service best practices.
/// </summary>
public abstract class SyncServiceBase<T> : CronJobHelper
{
    /// <summary>
    ///     The logger instance for logging service activity.
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    ///     The service scope factory for resolving scoped services.
    /// </summary>
    protected readonly IServiceScopeFactory ScopeFactory;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SyncServiceBase{T}" /> class with the specified schedule
    ///     configuration,
    ///     logger, and scope factory.
    /// </summary>
    /// <param name="config">The schedule configuration for the cron job.</param>
    /// <param name="logger">The logger instance for logging service activity.</param>
    /// <param name="scopeFactory">The service scope factory for resolving scoped services.</param>
    protected SyncServiceBase(IScheduleConfig<T> config, ILogger logger, IServiceScopeFactory scopeFactory)
        : base(config.CronExpression, config.TimeZoneInfo)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        ScopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    /// <summary>
    ///     Override this method to implement your sync logic.
    ///     Use ScopeFactory to resolve scoped services as needed.
    ///     Always handle exceptions and log appropriately.
    /// </summary>
    protected abstract Task ExecuteSyncAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Called by the cron scheduler. Handles error logging and calls <see cref="ExecuteSyncAsync" />.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    public override async Task DoWork(CancellationToken cancellationToken)
    {
        try
        {
            Logger.LogInformation($"{DateTime.Now:hh:mm:ss} {GetType().Name} is working.");
            await ExecuteSyncAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during sync job execution.");
        }
    }
}