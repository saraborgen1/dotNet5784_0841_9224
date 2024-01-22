namespace Dal;
using DalApi;
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }

    /// <summary>
    /// Interface property implement
    /// </summary>
    public ITask Task => new TaskImplementation();
    /// <summary>
    /// Interface property implement
    /// </summary>
    public IDependency Dependency => new DependencyImplementation();
    /// <summary>
    /// Interface property implement
    /// </summary>
    public IEngineer Engineer => new EngineerImplementation();
}

