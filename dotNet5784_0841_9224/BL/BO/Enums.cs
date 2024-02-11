namespace BO;

public class Enums
{
    public enum EngineerExperience
    {
        Beginner,
        AdvancedBeginner,
        Intermediate,
        Advanced,
        Expert,
        None//default for filter in pl
    }
    public enum Status
    {
        Unscheduled, 
        Scheduled, 
        OnTrack, 
        Done
    }
    public enum ProjectStatus
    {
        Creation,
        Scheduling,
        Start
    }
}
