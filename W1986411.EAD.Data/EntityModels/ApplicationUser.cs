using Microsoft.AspNetCore.Identity;

namespace W1986411.EAD.Data;

/// <summary>
/// Application user.
/// </summary>
/// <seealso cref="IdentityUser" />
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Email
    /// </summary>
    public override string Email { get => base.Email; set => base.Email = value; }

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    public double Height { get; set; }
}
