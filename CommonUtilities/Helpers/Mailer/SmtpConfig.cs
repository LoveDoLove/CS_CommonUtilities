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