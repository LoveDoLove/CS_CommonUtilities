namespace CommonUtilities.Helpers.Mailer;

public interface IMailerHelper
{
    Task<bool> SendEmail(MailerConfig mail);
}