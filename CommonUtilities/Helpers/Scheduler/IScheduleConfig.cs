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

namespace CommonUtilities.Helpers.Scheduler;

/// <summary>
///     Defines the schedule configuration interface for a cron job, including the cron expression and time zone.
/// </summary>
public interface IScheduleConfig<T>
{
    /// <summary>
    ///     Gets or sets the cron expression that defines the job schedule.
    /// </summary>
    string CronExpression { get; set; }

    /// <summary>
    ///     Gets or sets the time zone information for the job schedule.
    /// </summary>
    TimeZoneInfo TimeZoneInfo { get; set; }
}