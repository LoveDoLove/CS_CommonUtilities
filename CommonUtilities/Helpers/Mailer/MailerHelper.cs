using MailKit.Net.Smtp;
using MimeKit;
using Serilog;

namespace CommonUtilities.Helpers.Mailer;

public class MailerHelper : IMailerHelper
{
    private readonly SmtpConfig _smtp;

    public MailerHelper(SmtpConfig smtp)
    {
        _smtp = smtp;
    }

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