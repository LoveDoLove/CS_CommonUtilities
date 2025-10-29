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