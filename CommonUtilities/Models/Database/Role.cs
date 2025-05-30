using System.ComponentModel.DataAnnotations;

namespace CommonUtilities.Models.Database;

/// <summary>
///     Defines the standard user roles within the application.
/// </summary>
public enum UserRoles
{
    /// <summary>
    ///     Represents a Super Administrator with the highest level of permissions.
    /// </summary>
    SuperAdmin,

    /// <summary>
    ///     Represents an Administrator with elevated permissions, typically below SuperAdmin.
    /// </summary>
    Admin,

    /// <summary>
    ///     Represents a standard Member or regular user with basic permissions.
    /// </summary>
    Member
}

/// <summary>
///     Represents a user role entity, typically stored in a database.
/// </summary>
public class Role
{
    /// <summary>
    ///     Gets or sets the unique identifier for the role.
    ///     This is typically the primary key in the database.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the name of the role (e.g., "SuperAdmin", "Admin", "Member").
    ///     The maximum length is 100 characters.
    /// </summary>
    /// <value>The role name. Defaults to an empty string.</value>
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}