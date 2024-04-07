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
    /// An propoty that represents the project start date
    /// </summary>
    public DateTime? StartProject { set; get; }
    /// <summary>
    /// An propoty that represents the project end date
    /// </summary
    public DateTime? EndProject { set; get; }
    /// <summary>
    /// A function that returns the project status
    /// </summary>
    /// <returns>project status</returns>
    public DO.ProjectStatus StatusProject();

    /// <summary>
    /// propoty of  Current time project
    /// </summary>
    public DateTime CurrentDate { set; get; }
    /// <summary>
    /// propoty of AdminPassword
    /// </summary>
    public string AdminPassword { get;}
    /// <summary>
    /// propoty of AdminUserId
    /// </summary>
    public int AdminUserId { get;}
}
