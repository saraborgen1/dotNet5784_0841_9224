namespace DO;

public record Task
(
    int Id,
    string? Ailas=null,
    string? Description=null,
    bool IsMilestone = false,
    DateTime? CreatedAtDate=null,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    TimeSpan RequiredEffortTime,
    string? Deliverables=null,
    string? Remarks = null,
    int? Engineerld=null,
    EngineerExperience? Copmlexity= EngineerExperience.Beginner
)
{
    public DateTime CreatedAtDate=>DateTime.Now;
    public DateTime StartDate => DateTime.Now;
    public DateTime ScheduledDate => DateTime.Now;
    public DateTime DeadlineDate => DateTime.Now;
    public DateTime CompleteDate => DateTime.Now;
    public TimeSpan RequiredEffortTime=>RequiredEffortTime;
}
