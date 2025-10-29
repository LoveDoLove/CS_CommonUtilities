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

namespace CommonUtilities.Helpers.Stripe;

/// <summary>
///     Represents the configuration settings required to authenticate with the Stripe API.
/// </summary>
public class StripeConfig
{
    /// <summary>
    ///     Gets or sets the Stripe API key used for authentication.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the Stripe Webhook secret used for validating webhook signatures.
    /// </summary>
    public string WebhookSecret { get; set; } = string.Empty;
}