namespace BlImplementation;
using BlApi;
using System.Data;
using System.Linq;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private const string _entityName = nameof(BO.Task);
    public void Create(BO.Task item)
    {
        if (item.Id <= 0) throw new BO.BlTheInputIsInvalidException("Id");
        if (item.Alias == null) throw new BO.BlTheInputIsInvalidException("Name");
        if(item.Description==null) throw new BO.BlTheInputIsInvalidException("Description");
        if(item.RequiredEffortTime==null||item.RequiredEffortTime< TimeSpan.Zero) throw new BO.BlTheInputIsInvalidException("RequiredEffortTime");
        if(item.Deliverables==null) throw new BO.BlTheInputIsInvalidException("Deliverables");
        if (item.Dependencies != null)
        {
            var temp = item.Dependencies.ToList().Select(p =>
            {
                _dal.Dependency.Create(new DO.Dependency(0, item.Id, p.Id));
                return p;
            });
        }

        DO.Task doTask = new DO.Task
      (item.Id, item.Alias, item.Description,DateTime.Now, null
      , null, null, null, item.RequiredEffortTime,
      item.Deliverables, item.Remarks, null, (DO.EngineerExperience)item.Copmlexity);

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
        List<int?> dependenciesId = (from item in _dal.Dependency.ReadAll(p => p.DependentOnTask == id)
                                     where item.DependentTask != null
                                     select item.DependentTask).ToList();
        if (dependenciesId != null)
            throw new BO.BlCannotBeDeletedException(id, _entityName);

        var temp = (_dal.Dependency.ReadAll(p => p.DependentTask == id)
            .ToList())
            .Select(item =>
        {
            _dal.Dependency.Delete(item.Id);
            return item;
        });

        _dal.Task.Delete(id);

    }
    public BO.Task Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(p => p.Id == id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException(id, _entityName);

        var gDependenciesId = (from temp in _dal.Dependency.ReadAll(p => p.DependentTask == id)
                               group temp by temp.DependentTask % id into gs
                               select gs).ToList();

        var dependenciesId = (from temp in gDependenciesId
                              where temp.Key == 0
                              from item in temp
                              select item.Id).ToList();

        List<BO.TaskInList>? dependencies = (from item in dependenciesId
                                             let alias = _dal.Task.Read(p => p.Id == item)?.Ailas ?? " "
                                             let description = _dal.Task.Read(p => p.Id == item)?.Description ?? " "
                                             let temp = (BO.Enums.Status)Read(item).Status
                                             select new BO.TaskInList()
                                             {
                                                 Id = item,
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

    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null!)
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
                var temp1 = item.Dependencies.Select(t =>
                {
                    var dependentTask = _dal.Task.Read(p => p.Id == t.Id);

                    if (dependentTask?.ScheduledDate == null)
                        throw new BO.BlNoDateException("The task it depends on doesn't have a scheduled date");

                    return t;
                }).ToList();

                var temp2 = item.Dependencies.Select(p =>
                {
                    if (Read(p.Id).ForecastDate > item.ScheduledDate)
                        throw new BO.BlDateClashException("The dependent task's start date is before the end date of the task it depends on");
                    return p;
                }).ToList();
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
            var temp1 = dependencies.Select(item =>
            {
                if (_dal.Task.Read(p => p.Id == item.Id)!.ScheduledDate == null)
                    throw new BO.BlNoDateException("The task it depends on doesnt have a scheduled date");
                return item;
            });
            var temp2 = dependencies.Select(item =>
            {
                if (Read(item.Id).ForecastDate > date)
                    throw new BO.BlDateClashException("The dependent task's start date is before the end date of the task it depends on");
                return item;
            });
            DO.Task newTask = _dal.Task.Read(p => p.Id == id)! with { ScheduledDate = date };
            _dal.Task.Update(newTask);

        }
    }
}
