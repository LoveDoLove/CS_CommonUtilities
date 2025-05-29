namespace CommonUtilities.Models;

/// <summary>
///     Represents an email message, including recipients, subject, and body.
///     This class is used to encapsulate the details needed to send an email.
/// </summary>
public class Mail
{
    /// <summary>
    ///     Gets or sets the array of primary recipient email addresses.
    /// </summary>
    /// <value>An array of strings, where each string is an email address. Defaults to an empty array.</value>
    public string[] To { get; set; } = [];

    /// <summary>
    ///     Gets or sets the array of carbon copy (CC) recipient email addresses.
    ///     This property is optional.
    /// </summary>
    /// <value>An array of strings, where each string is an email address, or <c>null</c> if no CC recipients.</value>
    public string[]? Cc { get; set; }

    /// <summary>
    ///     Gets or sets the array of blind carbon copy (BCC) recipient email addresses.
    ///     This property is optional.
    /// </summary>
    /// <value>An array of strings, where each string is an email address, or <c>null</c> if no BCC recipients.</value>
    public string[]? Bcc { get; set; }

    /// <summary>
    ///     Gets or sets the subject line of the email.
    /// </summary>
    /// <value>The email subject. Defaults to an empty string.</value>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the body content of the email.
    ///     This can be plain text or HTML, depending on the mailer service configuration.
    /// </summary>
    /// <value>The email body. Defaults to an empty string.</value>
    public string Body { get; set; } = string.Empty;
}