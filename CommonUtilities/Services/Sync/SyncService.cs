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

using CommonUtilities.Helpers.Scheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CommonUtilities.Services.Sync;

/// <summary>
///     Represents a scheduled background service that performs periodic synchronization tasks using a cron schedule.
/// </summary>
public class SyncService : CronJobHelper
{
    private readonly ILogger<SyncService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SyncService" /> class with the specified schedule configuration,
    ///     logger, and scope factory.
    /// </summary>
    /// <param name="config">The schedule configuration for the cron job.</param>
    /// <param name="logger">The logger instance for logging service activity.</param>
    /// <param name="scopeFactory">The service scope factory for resolving scoped services.</param>
    public SyncService(IScheduleConfig<SyncService> config, ILogger<SyncService> logger,
        IServiceScopeFactory scopeFactory)
        : base(config.CronExpression, config.TimeZoneInfo)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    /// <summary>
    ///     Starts the synchronization service and schedules the first job occurrence.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(SyncService)} starts.");
        return base.StartAsync(cancellationToken);
    }

    /// <summary>
    ///     Performs the main synchronization work when triggered by the cron schedule.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    public override Task DoWork(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{DateTime.Now:hh:mm:ss} {nameof(SyncService)} is working.");

        using IServiceScope scope = _scopeFactory.CreateScope();
        //IResetPasswordService resetPasswordService = scope.ServiceProvider.GetRequiredService<IResetPasswordService>();

        _logger.LogInformation("Syncing status");
        //await resetPasswordService.SyncExpiredResetPasswordAsync();

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Stops the synchronization service and releases any running resources.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(SyncService)} is stopping.");
        return base.StopAsync(cancellationToken);
    }
}