namespace BO;

public class Enums
{
    /// <summary>
    /// engineer level enum
    /// </summary>
    public enum EngineerExperience
    {
        Beginner,
        AdvancedBeginner,
        Intermediate,
        Advanced,
        Expert,
        None//default for filter in pl
    }

    /// <summary>
    /// task status enum
    /// </summary>
    public enum Status
    {
        Unscheduled, 
        Scheduled, 
        OnTrack, 
        Done
    }

    /// <summary>
    /// project status enum
    /// </summary>
    public enum ProjectStatus
    {
        Creation,
        Scheduling,
        Start
    }
}
