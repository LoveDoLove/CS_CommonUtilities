using System.ComponentModel.DataAnnotations;

namespace CommonUtilities.Models.Database;

/// <summary>
///     Represents a user entity in the application, typically stored in a database.
///     Contains user profile information, credentials, and status flags.
/// </summary>
public class User
{
    /// <summary>
    ///     Gets or sets the unique identifier for the user.
    ///     This is typically the primary key in the database.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the user's first name.
    ///     Maximum length is 100 characters.
    /// </summary>
    /// <value>The user's first name. Defaults to an empty string.</value>
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the user's last name.
    ///     Maximum length is 100 characters.
    /// </summary>
    /// <value>The user's last name. Defaults to an empty string.</value>
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the username chosen by the user.
    ///     Maximum length is 255 characters.
    /// </summary>
    /// <value>The username. Defaults to an empty string.</value>
    [MaxLength(255)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the user's email address.
    ///     Maximum length is 255 characters.
    /// </summary>
    /// <value>The user's email address. Defaults to an empty string.</value>
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the hashed password for the user.
    ///     Maximum length is 255 characters (to accommodate various hashing algorithm outputs).
    /// </summary>
    /// <value>The hashed password. Defaults to an empty string.</value>
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the user's phone number.
    ///     Maximum length is 50 characters.
    /// </summary>
    /// <value>The user's phone number. Defaults to an empty string.</value>
    [MaxLength(50)]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the role assigned to the user (e.g., "Admin", "Member").
    ///     Maximum length is 50 characters.
    ///     Defaults to the string representation of <see cref="UserRoles.Member" />.
    /// </summary>
    /// <value>The user's role. Defaults to "Member".</value>
    [MaxLength(50)]
    public string Role { get; set; } = UserRoles.Member.ToString();

    /// <summary>
    ///     Gets or sets the URL of the user's profile image.
    ///     This property is optional. Maximum length is 255 characters.
    /// </summary>
    /// <value>The URL of the profile image, or <c>null</c> if not set.</value>
    [MaxLength(255)]
    public string? ProfileImageUrl { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the user's account is blocked.
    ///     A blocked user typically cannot log in or access certain features.
    /// </summary>
    /// <value><c>true</c> if the user is blocked; otherwise, <c>false</c>. Defaults to <c>false</c>.</value>
    public bool IsBlocked { get; set; } = false;

    /// <summary>
    ///     Gets or sets a value indicating whether the user's account is marked as deleted (soft delete).
    ///     A value of 0 typically means not deleted, while a non-zero value (e.g., 1) means deleted.
    /// </summary>
    /// <value>0 if not deleted, 1 (or other non-zero) if soft-deleted. Defaults to 0.</value>
    public int IsDeleted { get; set; } = 0; // Consider using bool for soft delete unless multiple states are needed.

    /// <summary>
    ///     Gets or sets a value indicating whether Multi-Factor Authentication (MFA) is enabled for the user.
    /// </summary>
    /// <value><c>true</c> if MFA is enabled; otherwise, <c>false</c>. Defaults to <c>false</c>.</value>
    public bool IsMfaEnabled { get; set; } = false;

    /// <summary>
    ///     Gets or sets the Google Authenticator secret key for the user if MFA is enabled.
    ///     Maximum length is 50 characters.
    /// </summary>
    /// <value>The Google MFA secret key. Defaults to an empty string.</value>
    [MaxLength(50)]
    public string GoogleMfaSecretKey { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the date and time when the user account was created.
    /// </summary>
    /// <value>The creation timestamp. Defaults to the current date and time.</value>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    ///     Gets or sets the date and time when the user account was last updated.
    /// </summary>
    /// <value>The last update timestamp. Defaults to the current date and time.</value>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Navigation properties
    // /// <summary>
    // /// Gets or sets the collection of shopping carts associated with this user.
    // /// This is a navigation property for Entity Framework Core.
    // /// </summary>
    // public ICollection<Cart> Carts { get; set; } = [];
}