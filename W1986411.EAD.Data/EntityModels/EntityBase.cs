namespace W1986411.EAD.Data;

/// <summary>
/// Entity base.
/// </summary>
public class EntityBase
{
    /// <summary>
    /// Gets or sets the created user.
    /// </summary>
    public string CreatedUser { get; set; }

    /// <summary>
    /// Gets or sets the created date time.
    /// </summary>
    public DateTime? CreatedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the modified user.
    /// </summary>
    public string ModifiedUser { get; set; }

    /// <summary>
    /// Gets or sets the modified date time.
    /// </summary>
    public DateTime? ModifiedDateTime { get; set; }
}
