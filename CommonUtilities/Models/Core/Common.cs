namespace CommonUtilities.Models.Core;

/// <summary>
///     Provides common constants and static values used throughout the application.
///     This includes application settings, validation patterns, error messages, and paths.
/// </summary>
public static class Common
{
    /// <summary>
    ///     The display name of the application.
    /// </summary>
    public const string AppName = "CommonUtilities";

    // Validation Constants
    /// <summary>
    ///     Regular expression for validating phone numbers.
    ///     Allows an optional '+' prefix, followed by 10 to 15 digits.
    /// </summary>
    public const string PhoneNumber = @"^(\+)?[0-9]{10,15}$";

    /// <summary>
    ///     Minimum required length for passwords.
    /// </summary>
    public const int PasswordLength = 8;

    // Error Message Constants
    /// <summary>
    ///     Generic error message format string. Example: "Invalid Email."
    /// </summary>
    public const string ErrorMessage = "Invalid {0}.";

    /// <summary>
    ///     Error message for invalid requests.
    /// </summary>
    public const string InvalidRequestErrorMessage = "Invalid request.";

    /// <summary>
    ///     Error message for passwords that do not meet the length requirement.
    /// </summary>
    public const string PasswordErrorMessage = "Password must be at least 8 characters long.";

    /// <summary>
    ///     Error message for when password and confirmation password do not match.
    /// </summary>
    public const string PasswordDoesNotMatchErrorMessage = "Password does not match.";

    /// <summary>
    ///     Error message for when an email address already exists in the system.
    /// </summary>
    public const string EmailExistsErrorMessage = "Email already exists.";

    /// <summary>
    ///     Error message format string for invalid phone numbers. Example: "Invalid Phone Number"
    /// </summary>
    public const string PhoneNumberErrorMessage = "Invalid {0}";

    /// <summary>
    ///     Error message for when a CAPTCHA response is required but not provided.
    /// </summary>
    public const string CaptchaErrorMessage = "Captcha is required.";

    /// <summary>
    ///     Message displayed when a user needs to log in to continue.
    /// </summary>
    public const string PleaseLoginToContinueErrorMessage = "Please login to continue.";

    // WWWRoot Path Constants
    /// <summary>
    ///     Relative path for general images within the wwwroot directory.
    /// </summary>
    public const string ImagePath = "images/";

    /// <summary>
    ///     Relative path for user-uploaded images within the wwwroot directory.
    /// </summary>
    public const string ImageUploadPath = "uploads/";
}

/// <summary>
///     Defines constants for authorization policy names used in the application.
///     These policy names are typically registered during application startup.
/// </summary>
public static class AuthorizationPolicies
{
    /// <summary>
    ///     Policy name for actions restricted to Super Administrators only.
    /// </summary>
    public const string SuperAdminOnly = "SuperAdminOnly";

    /// <summary>
    ///     Policy name for actions requiring Administrator privileges or higher.
    /// </summary>
    public const string RequireAdminOrHigher = "RequireAdminOrHigher";

    /// <summary>
    ///     Policy name for actions requiring any authenticated user.
    /// </summary>
    public const string AuthenticatedUsersOnly = "AuthenticatedUsersOnly";

    /// <summary>
    ///     Policy name for actions restricted to regular Users (non-admin).
    /// </summary>
    public const string UserOnly = "UserOnly";
}

/// <summary>
///     Provides constants related to payment initialization, such as currency, tax rates, and service charges.
/// </summary>
public static class PaymentInit
{
    /// <summary>
    ///     The default currency code for payments (e.g., "myr" for Malaysian Ringgit).
    /// </summary>
    public const string Currency = "myr";

    /// <summary>
    ///     The standard tax rate percentage (e.g., 8 for 8%).
    /// </summary>
    public const decimal TaxRate = 8;

    /// <summary>
    ///     The standard service charge percentage (e.g., 10 for 10%).
    /// </summary>
    public const decimal ServiceCharge = 10;
}

/// <summary>
///     Provides constants for SweetAlert message types or categories.
///     These are typically used for client-side alert styling.
/// </summary>
public static class SweetAlert
{
    /// <summary>Represents an informational alert type.</summary>
    public static readonly string Info = "Info";

    /// <summary>Represents a warning alert type.</summary>
    public static readonly string Warning = "Warning";

    /// <summary>Represents a success alert type.</summary>
    public static readonly string Success = "Success";

    /// <summary>Represents a danger or error alert type.</summary>
    public static readonly string Danger = "Danger";
}

/// <summary>
///     Provides constants for email subject lines related to various application events.
/// </summary>
/// <remarks>
///     Author: LoveDoLove
///     Description: Email subjects for OTP, Reset Password, Illegal Login, Password Changed, etc.
/// </remarks>
public static class EmailDesign
{
    /// <summary>Subject line for emails containing a one-time token.</summary>
    public const string TokenSubject = "Token";

    /// <summary>Subject line for password reset emails.</summary>
    public const string ResetPasswordSubject = "Reset Password";

    /// <summary>Subject line for emails notifying about a suspected illegal login attempt.</summary>
    public const string IllegalLoginSubject = "Illegal Login";

    /// <summary>Subject line for emails confirming a password change.</summary>
    public const string PasswordChangedSubject = "Password Changed";

    /// <summary>Subject line for emails sent upon account creation.</summary>
    public const string AccountCreatedSubject = "Account Created";

    /// <summary>Subject line for login alert notifications.</summary>
    public const string LoginAlertSubject = "Login Alert";

    /// <summary>Subject line for MFA (Multi-Factor Authentication) related alerts.</summary>
    public const string MfaAlertSubject = "MFA Alert";

    /// <summary>Subject line for emails related to resetting MFA.</summary>
    public const string ResetMfaSubject = "Reset MFA";

    /// <summary>Subject line for general payment receipt emails.</summary>
    public const string PaymentReceiptSubject = "Payment Receipt";

    /// <summary>Subject line for Stripe-specific payment receipt emails.</summary>
    public const string
        StripePaymentReceiptSubject =
            "Stripe"; // Consider making this more descriptive, e.g., "Your Stripe Payment Receipt"

    /// <summary>Subject line for emails regarding an order cancellation request.</summary>
    public const string OrderCancelRequestSubject = "Order Cancel Request";

    /// <summary>Subject line for emails confirming an order has been refunded.</summary>
    public const string OrderRefundedSubject = "Order Refunded";
}