namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;
using System.Data;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private const string _entityName = nameof(BO.Task);
    public void Create(BO.Task item)
    {
        if (item.Id <= 0) throw new ArgumentException();
        if (item.Alias == " ") throw new ArgumentException();

        if (item.Dependencies != null)
            item.Dependencies.ToList().ForEach(p => _dal.Dependency.Create(new DO.Dependency(0, item.Id, p.Id)));

        DO.Task doTask = new DO.Task
      (item.Id, item.Alias, item.Description, item.CreatedAtDate, item.StartDate
      , item.ScheduledDate, item.DeadlineDate, item.CompleteDate, item.RequiredEffortTime,
      item.Deliverables, item.Remarks, item.Engineer?.Id ?? 0, (DO.EngineerExperience)item.Copmlexity);

        try
        {
            int idTask = _dal.Task.Create(doTask);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistException(ex);
        }

    }

    public void Delete(int id)
    {
        try
        {
            BO.Task boTask = Read(id);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlCannotBeDeletedException(id, _entityName, ex);
        }
        List<int> dependenciesId = (from item in _dal.Dependency.ReadAll(p => p.DependentOnTask == id)
                                    select item.DependentTask ?? 0).ToList();
        if (dependenciesId != null)
            throw new BO.BlCannotBeDeletedException(id, _entityName);

        _dal.Dependency.ReadAll(p => p.DependentTask == id).ToList().ForEach(item => _dal.Dependency.Delete(item.Id));

        _dal.Task.Delete(id);

    }
    public BO.Task Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(p => p.Id == id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException(id, _entityName);

        List<int?>? dependenciesId = (from item in _dal.Dependency.ReadAll(p => p.DependentTask == id)
                                      select item.DependentOnTask).ToList();

        List<BO.TaskInList>? dependencies = (from item in dependenciesId
                                             let alias = _dal.Task.Read(p => p.Id == item)?.Ailas ?? " "
                                             let description = _dal.Task.Read(p => p.Id == item)?.Description ?? " "
                                             let temp = (BO.Enums.Status)Read(item ?? 0).Status
                                             select new BO.TaskInList()
                                             {
                                                 Id = item.Value,
                                                 Alias = alias,
                                                 Description = description,
                                                 Status = temp
                                             }).ToList();


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
            Dependencies = dependencies,
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            ForecastDate = forecastDate,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Copmlexity = (BO.Enums.EngineerExperience)doTask.Copmlexity
        };
    }

    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool> filter = null!)
    {
        var doTaskList = (from DO.Task doTask in _dal.Task.ReadAll()
                          select Read(doTask.Id)).ToList();

        if (filter != null)
            return (from item in doTaskList
                    where filter(item)
                    select item).ToList();

        return doTaskList;
    }

    public void Update(BO.Task item)
    {

        var doTask = _dal.Task.Read(p => p.Id == item.Id);
        if (doTask == null) throw new BO.BlDoesNotExistException(item.Id, _entityName);
        if (item.ScheduledDate != null)
            if (item.Dependencies != null)
            {
                (item.Dependencies).ForEach(t =>
                {
                    if (_dal.Task.Read(p => p.Id == t.Id)!.ScheduledDate == null)
                        throw new BO.BlNoDateException("The task it depends on doesnt have a scheduled date");
                });
                (item.Dependencies).ForEach(p =>
                {
                    if (Read(p.Id).ForecastDate > item.ScheduledDate)
                        throw new BO.BlDateClashException("The dependent task's start date is before the end date of the task it depends on");
                });
            }
        if (item.DeadlineDate != null && item.ScheduledDate != null && item.DeadlineDate < item.ScheduledDate)
            throw new BO.BlDateClashException("The end date is before the start date");


        DO.Task updatedTask = new DO.Task
        (item.Id, item.Alias, item.Description, item.CreatedAtDate, item.StartDate
        , item.ScheduledDate, item.DeadlineDate, item.CompleteDate, item.RequiredEffortTime,
         item.Deliverables, item.Remarks, item.Engineer?.Id ?? 0, (DO.EngineerExperience)item.Copmlexity);

        try
        {
            _dal.Task.Update(updatedTask);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlCannotBeDeletedException(ex);
        }

    }

    public void UpdateDate(int id, DateTime date)
    {

        BO.Task? boTask = Read(id);
        var dependencies = boTask.Dependencies;
        if (dependencies != null)
        {
            foreach (var item in dependencies)
            {
                if (_dal.Task.Read(p => p.Id == item.Id)!.ScheduledDate == null)
                    throw new BO.BlNoDateException("The task it depends on doesnt have a scheduled date");
            }
            foreach (var item in dependencies)
            {
                if (BO.Task.Read(item.Id).ForecastDate > date)
                    throw new BO.BlDateClashException("The dependent task's start date is before the end date of the task it depends on");
            }
            DO.Task newTask = _dal.Task.Read(p => p.Id == id)! with { ScheduledDate = date };
            _dal.Task.Update(newTask);

        }
    }
}
