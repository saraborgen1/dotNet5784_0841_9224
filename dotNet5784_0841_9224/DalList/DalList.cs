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

    /// <summary>
    /// An propotie that represents the project start date
    /// </summary>
    public DateTime? StartProject { get => DataSource.Config.startProject; set => DataSource.Config.startProject = value; }

    /// <summary>
    /// An propotie that represents the project end date
    /// </summary
    public DateTime? EndProject { get => DataSource.Config.endProject; set => DataSource.Config.endProject = value; }

    /// <summary>
    /// A function that returns the project status
    /// </summary>
    /// <returns>project status</returns>
    public ProjectStatus StatusProject()
    {
        if (StartProject == null)
            return ProjectStatus.Creation;
        if (Task.Read(p => p.StartDate == null) != null)
            return ProjectStatus.Scheduling;
        return ProjectStatus.Start;
    }
    /// <summary>
    /// propotie of  Current time project
    /// </summary>

    public DateTime? CurrentDate { get => DataSource.Config.currentDate; set => DataSource.Config.currentDate = value; }
}

