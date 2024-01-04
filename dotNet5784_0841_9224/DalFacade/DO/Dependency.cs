namespace DO;
/// <summary>
/// Dependency Entity represents a Dependency with all its props
/// </summary>
/// <param name="Id">Personal unique ID of the dependency (created automatically) </param>
/// <param name="DependentTask">ID number of pending task</param>
/// <param name="DependentOnTask">Previous assignment ID number</param>
public record Dependency
(
    int Id,
    int? DependentTask=null,
    int? DependentOnTask=null
)
//No parameter constructor was defined as it is autosettly defined
{
    /// <summary>
    /// empty ctor for stage 3
    /// </summary>
    public Dependency() : this(0) { } 
}
