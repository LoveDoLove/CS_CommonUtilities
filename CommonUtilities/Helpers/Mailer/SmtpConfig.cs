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

namespace CommonUtilities.Helpers.Mailer;

/// <summary>
///     Represents the SMTP server configuration required for sending emails.
/// </summary>
public class SmtpConfig
{
    /// <summary>
    ///     Gets or sets the sender's email address.
    /// </summary>
    public string From { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the SMTP server host name or IP address.
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the display name for the sender.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the password for SMTP authentication.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the SMTP server port number.
    /// </summary>
    public int Port { get; set; } = 587;

    /// <summary>
    ///     Gets or sets the username for SMTP authentication.
    /// </summary>
    public string Username { get; set; } = string.Empty;
}