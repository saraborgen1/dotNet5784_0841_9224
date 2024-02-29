namespace DO;
/// <summary>
/// A task entity represents the assigned tasks with everything included in the matter-times, output, notes, etc.
/// </summary>
/// <param name="Id">Unique ID number (automatic runner number)</param>
/// <param name="Ailas">Name of the task</param>
/// <param name="Description">A text detailing the task</param>
/// <param name="StartDate">Indicates the time when the task was created by the administrator</param>
/// <param name="ScheduledDate">Planned date for the start of work</param>
/// <param name="CompleteDate">Actual end date</param>
/// <param name="RequiredEffortTime">The length of time to proceed with the task</param>
/// <param name="Deliverables">A string describing the results or items provided at the end of the task</param>
/// <param name="Remarks">Notes on the assignment</param>
/// <param name="EngineerId">The engineer ID assigned to the task</param>
/// <param name="Copmlexity">The difficulty level of the task</param>
public record Task
(
    int Id,
    string? Ailas=null,
    string? Description=null,
    DateTime? CreatedAtDate=null,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    TimeSpan? RequiredEffortTime =null,
    string? Deliverables=null,
    string? Remarks = null,
    int? EngineerId=null,
    EngineerExperience Copmlexity= EngineerExperience.Beginner
)
{
    public Task() : this(0) { } //empty ctor for stage 3
}
