namespace DalApi;

public interface IDal
{
    /// <summary>
    /// property of task interface
    /// </summary>
    ITask Task { get; }
    /// <summary>
    /// property of dependency interface
    /// </summary>
    IDependency Dependency { get; }
    /// <summary>
    /// property of engineer interface
    /// </summary>
    IEngineer Engineer { get; }
    /// <summary>
    /// An propotie that represents the project start date
    /// </summary>
    public DateTime? StartProject { set; get; }
    /// <summary>
    /// An propotie that represents the project end date
    /// </summary
    public DateTime? EndProject { set; get; }
    /// <summary>
    /// A function that returns the project status
    /// </summary>
    /// <returns>project status</returns>
    public DO.ProjectStatus StatusProject();

    /// <summary>
    /// propotie of  Current time project
    /// </summary>
    public DateTime CurrentDate { set; get; }
}
