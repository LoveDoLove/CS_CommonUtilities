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

using Cronos;
using Microsoft.Extensions.Hosting;
using Timer = System.Timers.Timer;

namespace CommonUtilities.Helpers.Scheduler;

/// <summary>
///     Provides a base class for implementing scheduled background jobs using cron expressions in ASP.NET Core.
/// </summary>
public abstract class CronJobHelper : IHostedService, IDisposable
{
    private readonly CronExpression _expression;
    private readonly TimeZoneInfo _timeZoneInfo;
    private Timer? _timer; // Nullable Timer

    /// <summary>
    ///     Initializes a new instance of the <see cref="CronJobHelper" /> class with the specified cron expression and time
    ///     zone.
    /// </summary>
    /// <param name="cronExpression">The cron expression that defines the job schedule.</param>
    /// <param name="timeZoneInfo">The time zone information for the job schedule.</param>
    protected CronJobHelper(string cronExpression, TimeZoneInfo timeZoneInfo)
    {
        _expression = CronExpression.Parse(cronExpression);
        _timeZoneInfo = timeZoneInfo;
    }

    /// <summary>
    ///     Releases the resources used by the cron job.
    /// </summary>
    public virtual void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this); // Suppress finalization as Dispose is called
    }

    /// <summary>
    ///     Starts the cron job and schedules the first occurrence.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        await ScheduleJob(cancellationToken);
    }

    /// <summary>
    ///     Stops the cron job and releases any running timers.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    public virtual async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Stop();
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Schedules the next occurrence of the cron job based on the cron expression and time zone.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
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

    /// <summary>
    ///     Executes the actual work of the cron job.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    public virtual async Task DoWork(CancellationToken cancellationToken)
    {
        // Placeholder for actual work.
        // Derived classes should override this method to perform their specific tasks.
        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken); // Example: Simulate work
    }
}