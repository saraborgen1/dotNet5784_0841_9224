namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Data;

internal class StateImplementation : IState
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// propotie of start date project
    /// </summary>
    public DateTime? StartProject
    {
        get
        {
            return _dal.StartProject;
        }
        set
        {
            _dal.StartProject = value;
        }

    }

    /// <summary>
    /// propotie of start date project
    /// </summary>
    public DateTime? EndProject
    {
        get
        {
            return _dal.EndProject;
        }
        set
        {
            _dal.EndProject = value;
        }
    }
    /// <summary>
    /// function that returns projest status
    /// </summary>
    /// <returns>projest status</returns>
    public BO.Enums.ProjectStatus StatusProject()
    {
        //return (BO.Enums.ProjectStatus)_dal.StatusProject();
        if (StartProject == null)
            return BO.Enums.ProjectStatus.Creation;
        if (_dal.Task.Read(p => p.ScheduledDate == null) != null)
            return BO.Enums.ProjectStatus.Scheduling;
        return BO.Enums.ProjectStatus.Start;
    }

    /// <summary>
    /// propotie of  Current time project
    /// </summary>
    public DateTime CurrentDate
    {
        get
        {
            return _dal.CurrentDate;
        }
        set
        {
            _dal.CurrentDate = value;
        }
    }

    /// <summary>
    /// a function to add year
    /// </summary>
    public void AddYear()
    {
        CurrentDate = CurrentDate.AddYears(1);
    }

    /// <summary>
    /// a function to add Month
    /// </summary>
    public void AddMonth()
    {
        CurrentDate = CurrentDate.AddMonths(1);
    }
    public void AddDay()
    {
        CurrentDate = CurrentDate.AddDays(1);
    }
    public void AddWeek()
    {
        CurrentDate = CurrentDate.AddDays(7);
    }

    public void SetProjectDates(DateTime startDate, DateTime endDate)
    {
        var _task = new TaskImplementation();
        var tasks = _task.ReadAll();
        DateTime max = startDate;
        foreach (var task in tasks)
        {
            DateTime temp = findEndDate(task, startDate);
            max = (max > temp) ? max : temp;
        }
        if (max > endDate)
            throw new BlDateClashException("Not enough time\n");
        StartProject = startDate;
        EndProject = endDate;

    }


    private DateTime findEndDate(BO.Task task, DateTime startDate)
    {
        var _task = new TaskImplementation();
        DateTime max = startDate;
        if (task.Dependencies!.Count == 0)
        {
            DateTime temp = startDate + (TimeSpan)task.RequiredEffortTime!;
            max = (max > temp) ? max : temp;
        }
        else
        {
            foreach (var dep in task.Dependencies)
            {
                try
                {
                    var depTask = _task.Read(dep.Id);
                    DateTime temp = findEndDate(depTask, startDate) + (TimeSpan)task.RequiredEffortTime! ;
                    max = (max > temp) ? max : temp;
                }
                catch (Exception ex)
                { throw new BlDoesNotExistException(ex); }
            }
        }
        return max;
    }
}