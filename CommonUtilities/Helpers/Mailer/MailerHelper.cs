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

using MailKit.Net.Smtp;
using MimeKit;
using Serilog;

namespace CommonUtilities.Helpers.Mailer;

/// <summary>
///     Provides helper methods for sending emails using SMTP configuration.
/// </summary>
public class MailerHelper : IMailerHelper
{
    private readonly SmtpConfig _smtp;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MailerHelper" /> class with the specified SMTP configuration.
    /// </summary>
    /// <param name="smtp">The SMTP configuration settings.</param>
    public MailerHelper(SmtpConfig smtp)
    {
        _smtp = smtp;
    }

    /// <summary>
    ///     Asynchronously sends an email using the provided mail configuration.
    /// </summary>
    /// <param name="mail">The mail configuration containing recipients, subject, and body.</param>
    /// <returns>True if the email was sent successfully; otherwise, false.</returns>
    public async Task<bool> SendEmail(MailerConfig mail)
    {
        MimeMessage message = new();

        // From
        message.From.Add(new MailboxAddress(_smtp.Name, _smtp.From));

        // To
        InternetAddressList toList = [];
        foreach (string to in mail.To) toList.Add(MailboxAddress.Parse(to));
        message.To.AddRange(toList);

        // Cc
        InternetAddressList ccList = [];
        if (mail.Cc != null)
            foreach (string cc in mail.Cc)
                ccList.Add(MailboxAddress.Parse(cc));

        message.Cc.AddRange(ccList);

        // Bcc
        InternetAddressList bccList = [];
        if (mail.Bcc != null)
            foreach (string bcc in mail.Bcc)
                bccList.Add(MailboxAddress.Parse(bcc));

        message.Bcc.AddRange(bccList);

        // Subject
        message.Subject = mail.Subject;

        // Body
        message.Body = new TextPart("html")
        {
            Text = mail.Body
        };

        try
        {
            using SmtpClient client = new();
            await client.ConnectAsync(_smtp.Host, _smtp.Port, false);
            await client.AuthenticateAsync(_smtp.Username, _smtp.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error sending email to {ToAddresses} with subject {Subject}", string.Join(",", mail.To),
                mail.Subject);
            return false;
        }
    }
}