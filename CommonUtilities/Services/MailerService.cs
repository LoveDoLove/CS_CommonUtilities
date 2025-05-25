using CommonUtilities.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using Serilog;

// Added for logging

namespace CommonUtilities.Services;

/// <summary>
/// Service for sending emails using SMTP.
/// </summary>
public class MailerService : IMailerService
{
    private readonly Smtp _smtp;

    /// <summary>
    /// Initializes a new instance of the <see cref="MailerService"/> class.
    /// </summary>
    /// <param name="smtp">The SMTP configuration settings.</param>
    public MailerService(Smtp smtp)
    {
        _smtp = smtp;
    }

    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="mail">The mail object containing email details (to, cc, bcc, subject, body).</param>
    /// <returns>A Task representing the asynchronous operation, with a result of true if the email was sent successfully, false otherwise.</returns>
    public async Task<bool> SendEmail(Mail mail)
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