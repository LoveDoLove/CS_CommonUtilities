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

using CommonUtilities.Helpers.Enums;
using Microsoft.EntityFrameworkCore;

namespace CommonUtilities.Models.Database;

/// <summary>
///     Represents the database context for the application, providing access to database tables
///     through <see cref="DbSet{TEntity}" /> properties and configuring the model.
/// </summary>
public class DB : DbContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DB" /> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext" />.</param>
    public DB(DbContextOptions<DB> options)
        : base(options)
    {
    }

    /// <summary>
    ///     Gets or sets the <see cref="DbSet{TEntity}" /> for the <see cref="Role" /> entities.
    ///     This represents the 'Roles' table in the database.
    /// </summary>
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    ///     Gets or sets the <see cref="DbSet{TEntity}" /> for the <see cref="User" /> entities.
    ///     This represents the 'Users' table in the database.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    ///     Configures the schema needed for the identity framework.
    ///     This method is called by the framework when the model for a derived context is being created.
    ///     It is used here to seed initial data, such as user roles.
    /// </summary>
    /// <param name="builder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Important to call the base method

        // Retrieve a list of all UserRoles enum values.
        // EnumHelper.ToList<UserRoles>() is assumed to be a custom helper method.
        List<UserRoles> userRoles = EnumHelper.ToList<UserRoles>();

        // Seed the 'Roles' table with data derived from the UserRoles enum.
        // Each role from the enum will be added as a new Role entity.
        // The Id is generated based on the index to ensure unique primary keys for seeded data.
        builder.Entity<Role>()
            .HasData(userRoles.Select((role, index) => new Role
            {
                Id = index + 1, // Assign a 1-based ID
                Name = role.ToString() // Use the enum member's string representation as the role name
            }));
    }

    // Example of adding another DbSet for a 'Product' entity:
    // /**
    //  * @ModelName: Product
    //  * @TableName: Products
    //  *
    //  * Remember Table Name Must Add "s" at the end
    //  */
    // public DbSet<Product> Products { get; set; }
}