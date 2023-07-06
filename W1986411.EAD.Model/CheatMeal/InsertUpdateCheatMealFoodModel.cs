﻿namespace W1986411.EAD.Model;

/// <summary>
/// Insert update cheat meal food model.
/// </summary>
public class InsertUpdateCheatMealFoodModel
{
    /// <summary>
    /// Gets or sets the food identifier.
    /// </summary>
    public int FoodId { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the calories gain.
    /// </summary>
    public double CaloriesGain { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    public bool IsActive { get; set; }
}
