namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Data;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(Task item)
    {
        if (item.Id <= 0) throw new ArgumentException();
        if (item.Alias == " ") throw new ArgumentException();

        DO.Task doTask = new DO.Task
      (item.Id, item.Alias, item.Description, false, item.CreatedAtDate, item.StartDate
      , item.ScheduledDate, item.DeadlineDate, item.CompleteDate, item.RequiredEffortTime,
      item.Deliverables, item.Remarks, item.Engineer.Id, item.Copmlexity);

        try
        {
            int idTask = _dal.Task.Create(doTask);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException( ex   );
        }

    }

    public void Delete(int id)
    {
        try
        {
            BO.Task boTask = Read(id);
            if (boTask.Dependencies == null) { throw new BO.BlDoesNotExistException(id, Task); }
            _dal.Task.Delete(id);
        }
        catch (Exception ex) { }
    }

    public Task Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(p => p.Id == id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException(id, "Task");
        List<int> dependenciesId = (from item in _dal.Dependency.ReadAll(p => p.DependentOnTask == id)
                                    select item.DependentTask ?? 0).ToList();
        List<BO.TaskInList> ? dependencies = (from item in dependenciesId
                                              select new BO.TaskInList()
                                              {
                                                  Id = item,
                                                  Alias = (_dal.Task.Read(p => p.Id == item)?.Ailas ?? " ",
                                                  Description=
                                                  //Description = (_dal.Task.Read(p => p.Id == item).Description,
                                                  //Status = BO.Enums.Status.Unscheduled   //Status
                                            }).ToList();

        List<int?>? dependentOnList = null!;
        dependentOnList = (from item in _dal.Dependency.ReadAll(p => p.DependentTask == id)
                           select item.DependentOnTask).ToList();
        DateTime? forecastDate = null;
        if (doTask.StartDate != null && doTask.ScheduledDate != null && doTask.RequiredEffortTime != null)
        {
            forecastDate = doTask.StartDate > doTask.ScheduledDate ? doTask.StartDate : doTask.ScheduledDate;
            forecastDate = (forecastDate + doTask.RequiredEffortTime);
        }
        BO.Enums.Status status;
        if (doTask.ScheduledDate == null)
            status = BO.Enums.Status.Unscheduled;
        else
            status = BO.Enums.Status.Scheduled;

        if (doTask.StartDate != null)
            status = BO.Enums.Status.OnTrack;
        if (doTask.CompleteDate != null)
            status = BO.Enums.Status.Done;
        return new BO.Task()
        {
            Id = doTask.Id,
            Alias = doTask.Ailas,
            Description = doTask.Description,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = status,
            //Dependencies = doTask.
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            ForecastDate = forecastDate,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Copmlexity = (BO.Enums.EngineerExperience)doTask.Copmlexity,
            DependentOnList = dependentOnList
        };
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool> filter = null!)
    {
        var doTaskList = (from DO.Task doTask in _dal.Task.ReadAll()
                          select Read(doTask.Id)).ToList();

        if (filter != null)
            return (from item in doTaskList
                    where filter(item)
                    select item).ToList();

        return doTaskList;
    }

    public void Update(Task item)
    {
        try
        {
            var doTask = _dal.Task.Read(p => p.Id == item.Id);
            //בדיקות תקינות להכל 
        }
        catch (Exception ex) { }

        DO.Task updatedTask = new DO.Task
        (item.Id, item.Alias, item.Description, false, item.CreatedAtDate, item.StartDate
        , item.ScheduledDate, item.DeadlineDate, item.CompleteDate, item.RequiredEffortTime,
         item.Deliverables, item.Remarks, item.Engineer.Id, item.Copmlexity);

        try
        {
            _dal.Task.Update(updatedTask);
        }
        catch (Exception ex) { }

    }

    public void UpdateDate(int id, DateTime date)
    {
        BO.Task? boTask = Read(id);
        if (boTask == null) { throw mlsdkc}
        List<BO.TaskInList>? dependencies = boTask.Dependencies;


        TaskInList.

    }
}
