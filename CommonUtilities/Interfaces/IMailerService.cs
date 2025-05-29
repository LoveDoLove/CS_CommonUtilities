namespace CommonUtilities.Interfaces;

/// <summary>
///     Defines the contract for a service that handles sending emails.
/// </summary>
public interface IMailerService
{
    /// <summary>
    ///     Asynchronously sends an email based on the provided <see cref="Mail" /> object.
    /// </summary>
    /// <param name="mail">
    ///     A <see cref="Mail" /> object containing all the necessary information for sending the email,
    ///     such as recipient(s), sender, subject, body, and any attachments.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains <c>true</c> if the email
    ///     was sent successfully (or queued for sending successfully); otherwise, <c>false</c>.
    /// </returns>
    Task<bool> SendEmail(Mail mail);
}