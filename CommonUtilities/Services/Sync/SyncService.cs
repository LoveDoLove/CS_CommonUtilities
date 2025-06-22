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
///     Example implementation of a scheduled background sync service.
///     To use: Register with AddHostedService
///     <SyncService>
///         and provide IScheduleConfig
///         <SyncService>
///             .
///             Override <see cref="ExecuteSyncAsync" /> for your custom logic.
/// </summary>
public class SyncService : SyncServiceBase<SyncService>
{
    public SyncService(IScheduleConfig<SyncService> config, ILogger<SyncService> logger,
        IServiceScopeFactory scopeFactory)
        : base(config, logger, scopeFactory)
    {
    }

    /// <inheritdoc />
    protected override Task ExecuteSyncAsync(CancellationToken cancellationToken)
    {
        using var scope = ScopeFactory.CreateScope();
        // var myService = scope.ServiceProvider.GetRequiredService<IMyService>();
        // await myService.DoSomethingAsync();
        Logger.LogInformation("Default sync logic executed. Override this in your own service.");
        return Task.CompletedTask;
    }
}

// Usage Example:
// services.AddHostedService<MyCustomSyncService>();
// public class MyCustomSyncService : SyncServiceBase<MyCustomSyncService> { ... override ExecuteSyncAsync ... }