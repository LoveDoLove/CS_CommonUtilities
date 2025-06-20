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