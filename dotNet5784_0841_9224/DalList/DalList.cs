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

    public DateTime? StartProject { get => DataSource.Config.startProject; set => DataSource.Config.startProject = value; }
    public DateTime? EndProject { get => DataSource.Config.endProject; set => DataSource.Config.endProject = value; }

    public ProjectStatus StatusProject()
    {
        if (StartProject == null)
            return ProjectStatus.Creation;
        if (Task.Read(p => p.StartDate == null) != null)
            return ProjectStatus.Scheduling;
        return ProjectStatus.Start;
    }
}

