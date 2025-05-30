namespace CommonUtilities.Models.Api;

/// <summary>
///     Represents SMTP (Simple Mail Transfer Protocol) server configuration settings.
///     This class is used to store details required to connect to an SMTP server for sending emails.
/// </summary>
public class Smtp
{
    /// <summary>
    ///     Gets or sets the default 'From' email address for outgoing emails.
    /// </summary>
    /// <value>The sender's email address. Defaults to an empty string.</value>
    public string From { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the hostname or IP address of the SMTP server.
    /// </summary>
    /// <value>The SMTP server host. Defaults to an empty string.</value>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the display name associated with the 'From' email address.
    /// </summary>
    /// <value>The sender's display name. Defaults to an empty string.</value>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the password for authenticating with the SMTP server.
    /// </summary>
    /// <value>The SMTP authentication password. Defaults to an empty string.</value>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the port number for the SMTP server.
    ///     Common ports are 25, 465 (SSL/TLS), or 587 (STARTTLS).
    /// </summary>
    /// <value>The SMTP server port. Defaults to 587.</value>
    public int Port { get; set; } = 587; // Default SMTP port for TLS/STARTTLS

    /// <summary>
    ///     Gets or sets the username for authenticating with the SMTP server.
    ///     This is often the same as the 'From' email address.
    /// </summary>
    /// <value>The SMTP authentication username. Defaults to an empty string.</value>
    public string Username { get; set; } = string.Empty;
}