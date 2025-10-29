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
///     Represents the configuration for an email message, including recipients, subject, and body.
/// </summary>
public class MailerConfig
{
    /// <summary>
    ///     Gets or sets the primary recipients of the email.
    /// </summary>
    public string[] To { get; set; } = [];

    /// <summary>
    ///     Gets or sets the CC (carbon copy) recipients of the email.
    /// </summary>
    public string[]? Cc { get; set; }

    /// <summary>
    ///     Gets or sets the BCC (blind carbon copy) recipients of the email.
    /// </summary>
    public string[]? Bcc { get; set; }

    /// <summary>
    ///     Gets or sets the subject of the email.
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the body content of the email (HTML supported).
    /// </summary>
    public string Body { get; set; } = string.Empty;
}