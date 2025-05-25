using Cronos;
using Microsoft.Extensions.Hosting;
using Timer = System.Timers.Timer;

namespace CommonUtilities.Helpers.Scheduler;

/// <summary>
/// Abstract base class for implementing a background service that executes a task based on a CRON schedule.
/// It uses <see cref="System.Timers.Timer"/> to schedule the work.
/// </summary>
public abstract class CronJobService : IHostedService, IDisposable
{
    private readonly CronExpression _expression;
    private readonly TimeZoneInfo _timeZoneInfo;
    private Timer? _timer; // Nullable Timer

    /// <summary>
    /// Initializes a new instance of the <see cref="CronJobService"/> class.
    /// </summary>
    /// <param name="cronExpression">The CRON expression string that defines the schedule.</param>
    /// <param name="timeZoneInfo">The time zone information to apply to the CRON schedule.</param>
    protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo)
    {
        _expression = CronExpression.Parse(cronExpression);
        _timeZoneInfo = timeZoneInfo;
    }

    /// <summary>
    /// Disposes the underlying timer.
    /// </summary>
    public virtual void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this); // Suppress finalization as Dispose is called
    }

    /// <summary>
    /// Starts the CRON job service. This method is called when the <see cref="IHostedService"/> starts.
    /// It schedules the first occurrence of the job.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous start operation.</returns>
    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        await ScheduleJob(cancellationToken);
    }

    /// <summary>
    /// Stops the CRON job service. This method is called when the <see cref="IHostedService"/> stops.
    /// It stops the timer, preventing further executions.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous stop operation.</returns>
    public virtual async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Stop();
        await Task.CompletedTask;
    }

    /// <summary>
    /// Schedules the next occurrence of the job based on the CRON expression.
    /// If the calculated delay is non-positive, it recursively calls itself to find the next valid future occurrence.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete and for job execution.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous scheduling operation.</returns>
    protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
    {
        // Get the next occurrence based on the current time and specified time zone
        DateTimeOffset? next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);

        if (next.HasValue)
        {
            var delay = next.Value - DateTimeOffset.Now;

            // If the calculated delay is in the past or immediate,
            // it might be due to clock adjustments or a very frequent cron.
            // Recursively schedule for the *next* valid time after this one.
            if (delay.TotalMilliseconds <= 0)
            {
                // Log this situation or handle it, then reschedule.
                // For now, we just immediately try to find the next slot.
                await ScheduleJob(cancellationToken);
                return; // Return to avoid setting up a timer with non-positive delay
            }

            _timer = new Timer(delay.TotalMilliseconds);
            _timer.Elapsed += async (sender, args) =>
            {
                // Stop and dispose the current timer as it has served its purpose for this occurrence
                _timer.Stop(); // Stop before disposing
                _timer.Dispose();
                _timer = null;

                // Execute the actual work if cancellation has not been requested
                if (!cancellationToken.IsCancellationRequested)
                {
                    await DoWork(cancellationToken);
                }

                // Reschedule the job for the next occurrence, if not cancelled
                if (!cancellationToken.IsCancellationRequested)
                {
                    await ScheduleJob(cancellationToken);
                }
            };
            _timer.Start();
        }
        // If next is null, the cron expression might be invalid or has no future occurrences.
        // This case should ideally be handled, e.g., by logging an error.
        await Task.CompletedTask;
    }

    /// <summary>
    /// The actual work to be performed by the CRON job. Derived classes must implement this method.
    /// This is a virtual method with a default placeholder implementation.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to stop the work.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public virtual async Task DoWork(CancellationToken cancellationToken)
    {
        // Placeholder for actual work.
        // Derived classes should override this method to perform their specific tasks.
        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken); // Example: Simulate work
    }
}