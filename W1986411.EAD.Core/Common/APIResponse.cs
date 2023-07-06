namespace W1986411.EAD.Core;

/// <summary>
/// API Response.
/// </summary>
public class APIResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether this instance is success.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    public object Data { get; set; }
}
