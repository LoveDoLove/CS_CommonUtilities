﻿// MIT License
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