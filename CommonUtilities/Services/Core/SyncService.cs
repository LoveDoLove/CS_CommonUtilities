using CommonUtilities.Utilities.Scheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CommonUtilities.Services.Core;

/// <summary>
///     Handles scheduled synchronization tasks.
///     This service inherits from <see cref="CronJobService" /> to execute tasks based on a CRON expression.
/// </summary>
/// <remarks>
///     Author: LoveDoLove
/// </remarks>
public class SyncService : CronJobService
{
    private readonly ILogger<SyncService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SyncService" /> class.
    /// </summary>
    /// <param name="config">The schedule configuration for this service.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="scopeFactory">The service scope factory.</param>
    /// <exception cref="ArgumentNullException">Thrown if logger or scopeFactory is null.</exception>
    public SyncService(IScheduleConfig<SyncService> config, ILogger<SyncService> logger,
        IServiceScopeFactory scopeFactory)
        : base(config.CronExpression, config.TimeZoneInfo)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    /// <summary>
    ///     Starts the synchronization service asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous start operation.</returns>
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(SyncService)} starts.");
        return base.StartAsync(cancellationToken);
    }

    /// <summary>
    ///     Performs the work of the synchronization service. This method is called at the scheduled time.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task" /> representing the work to be done.</returns>
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
    ///     Stops the synchronization service asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous stop operation.</returns>
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(SyncService)} is stopping.");
        return base.StopAsync(cancellationToken);
    }
}