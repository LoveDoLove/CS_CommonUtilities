using Cronos;
using Microsoft.Extensions.Hosting;
using Timer = System.Timers.Timer;

namespace CommonUtilities.Helpers.Scheduler;

public abstract class CronJobHelper : IHostedService, IDisposable
{
    private readonly CronExpression _expression;
    private readonly TimeZoneInfo _timeZoneInfo;
    private Timer? _timer; // Nullable Timer

    protected CronJobHelper(string cronExpression, TimeZoneInfo timeZoneInfo)
    {
        _expression = CronExpression.Parse(cronExpression);
        _timeZoneInfo = timeZoneInfo;
    }

    public virtual void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this); // Suppress finalization as Dispose is called
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        await ScheduleJob(cancellationToken);
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Stop();
        await Task.CompletedTask;
    }

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
                if (!cancellationToken.IsCancellationRequested) await DoWork(cancellationToken);

                // Reschedule the job for the next occurrence, if not cancelled
                if (!cancellationToken.IsCancellationRequested) await ScheduleJob(cancellationToken);
            };
            _timer.Start();
        }

        // If next is null, the cron expression might be invalid or has no future occurrences.
        // This case should ideally be handled, e.g., by logging an error.
        await Task.CompletedTask;
    }

    public virtual async Task DoWork(CancellationToken cancellationToken)
    {
        // Placeholder for actual work.
        // Derived classes should override this method to perform their specific tasks.
        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken); // Example: Simulate work
    }
}