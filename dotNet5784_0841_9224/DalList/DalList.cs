namespace Dal;
using DalApi;
using DO;
using System;

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

    public DateTime? StartProject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTime? EndProject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public ProjectStatus StatusProject()
    {
        throw new NotImplementedException();
    }
}

