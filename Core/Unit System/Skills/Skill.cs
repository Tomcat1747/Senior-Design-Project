/// <summary>
/// Class representing an "upgrade" to a unit.
/// </summary>
public interface Skill
{
    /// <summary>
    /// Determines how long the skill should last (expressed in turns). If set to negative number, skill will be permanent.
    /// </summary>
    int Duration { get; set; }

    /// <summary>
    /// Describes how the unit should be upgraded.
    /// </summary>
    void Apply(Unit unit);
    /// <summary>
    /// Returns units stats to normal.
    /// </summary>
    void Undo(Unit unit);

    /// <summary>
    /// Returns deep copy of the Skill object.
    /// </summary>
    Skill Clone();
}