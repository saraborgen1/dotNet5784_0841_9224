namespace DO;
/// <summary>
/// Engineer entity represents an engineer with all its characteristics - its identifying details
/// </summary>
/// <param name="Id">Personal unique ID of the Engineer (as in national id card)</param>
///<param name="Password">Private Password of the Engineer </param>
/// <param name="Name">Private Name of the Engineer </param>
/// <param name="Email">Engineer's personal email address</param>
/// <param name="Level">The level of the engineer in the project</param>
/// <param name="Cost">The cost of the engineer for an hour of work</param>
/// <param name="Active">If the engineer is active</param>
public record Engineer
(
    int Id,
    string? Password =null,
    string? Name = null,
    string? Email = null,
    EngineerExperience Level= EngineerExperience.Beginner,
    double? Cost = null

)
{
    public bool Active { get; set; } = true;
    //No parameter constructor was defined as it is autosettly defined

    /// <summary>
    /// empty ctor ctor for stage 3
    /// </summary>
    public Engineer() : this(0) { } 

}
