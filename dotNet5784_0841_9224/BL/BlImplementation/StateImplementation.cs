namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Xml.Linq;
using static Dal.XMLTools;
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
        return (BO.Enums.ProjectStatus)_dal.StatusProject();
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
        CurrentDate=CurrentDate.AddYears(1);
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

    public void AutoScheduling()
    {
        var tasks = ReadAll();
        foreach (var task in tasks) 
        {
            if(task.StartDate==null)
                setAutoDate(task);
        }
    }

    private void setAutoDate(DO.Task task)
    {
        if()
    }
}
