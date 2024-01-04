namespace DO;

public record Task
(
    int Id,
    string? Ailas=null,
    string? Description=null,
    bool IsMilestone = false,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    TimeSpan? RequiredEffortTime =null,
    string? Deliverables=null,
    string? Remarks = null,
    int? Engineerld=null,
    EngineerExperience? Copmlexity= EngineerExperience.Beginner
)
{
    public Task() : this(0) { } //empty ctor for stage 3
    public DateTime CreatedAtDate=>DateTime.Now;
}
