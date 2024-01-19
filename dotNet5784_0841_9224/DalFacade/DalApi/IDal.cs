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
}
