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