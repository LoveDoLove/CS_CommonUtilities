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

namespace CommonUtilities.Models;

/// <summary>
///     Represents the parameters for a console read line operation,
///     including the prompt question, a default value for cancellation,
///     and whether empty input is allowed.
/// </summary>
public class ReadLineModel
{
    /// <summary>
    ///     Gets or sets the question or prompt message to display to the user.
    /// </summary>
    /// <value>The question string. Defaults to <see cref="string.Empty" />.</value>
    public string Question { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the default value to be used if the user cancels the input.
    /// </summary>
    /// <value>The default value string. Defaults to <see cref="string.Empty" />.</value>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a value indicating whether empty or whitespace input is allowed.
    /// </summary>
    /// <value>True if empty input is allowed; otherwise, false. Defaults to false.</value>
    public bool AllowedEmpty { get; set; } = false;
}