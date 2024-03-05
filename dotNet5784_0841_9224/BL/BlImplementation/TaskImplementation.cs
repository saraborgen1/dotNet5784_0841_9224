namespace BlImplementation;
using BlApi;
using BO;
using System.Data;
using System.Linq;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private const string _entityName = nameof(BO.Task);
    private IState state = new StateImplementation();

    /// <summary>
    /// checks if everything is valid for the stage
    /// </summary>
    /// <param name="item">An object of type Bo</param>
    /// <exception cref="BO.BlCannotAddWrongStateException">Cannot Add Wrong State</exception>
    /// <exception cref="BO.BlTheInputIsInvalidException">The Input Is Invalid</exception>
    private void validation(BO.Task item)
    {
        if (state.StatusProject() == BO.Enums.ProjectStatus.Start)
            throw new BO.BlCannotAddWrongStateException("Cannot create a task at this state");
        if (state.StatusProject() != BO.Enums.ProjectStatus.Creation && item.Id <= 0) throw new BO.BlTheInputIsInvalidException("Id");
        if (item.Alias == null) throw new BO.BlTheInputIsInvalidException("Name");
        if (item.Description == null) throw new BO.BlTheInputIsInvalidException("Description");
        if (item.RequiredEffortTime == null || item.RequiredEffortTime < TimeSpan.Zero) throw new BO.BlTheInputIsInvalidException("RequiredEffortTime");
        if (item.Deliverables == null) throw new BO.BlTheInputIsInvalidException("Deliverables");
    }

    /// <summary>
    /// Adding a new object to  database
    /// </summary>
    /// <param name="item">An object of type Bo</param>
    /// <exception cref="BO.BlAlreadyExistException">Already Exist</exception>
    public void Create(BO.Task item)
    {
        validation(item);

        DO.Task doTask = new DO.Task
      (item.Id, item.Alias, item.Description, DateTime.Now, null
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
        if (item.Dependencies != null)
        {
            var temp = item.Dependencies.ToList().Select(p =>
            {
                _dal.Dependency.Create(new DO.Dependency(0, item.Id, p.Id));
                return p;
            });
        }

    }

    /// <summary>
    /// Deletion of an existing object with a certain ID, from the list of entity type objects.
    /// </summary>
    /// <param name="id">condition</param>
    /// <exception cref="BO.BlCannotBeDeletedWrongStateException">Cannot Be Deleted Wrong State</exception>
    /// <exception cref="BO.BlCannotBeDeletedException">Cannot Be Deleted</exception>
    public void Delete(int id)
    {
        if (state.StatusProject() == BO.Enums.ProjectStatus.Start)
            throw new BO.BlCannotBeDeletedWrongStateException("Cannot delete a task at this state");
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

    /// <summary>
    /// Returns an entity from the list that id matches param
    /// </summary>
    /// <param name="id">condition</param>
    /// <returns>an entity from the list that meets the condition</returns>
    /// <exception cref="BO.BlDoesNotExistException">Does Not Exist</exception>
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

        var dependentOn = (from temp in _dal.Dependency.ReadAll(p => p.DependentTask == id)
                           select temp.DependentOnTask).ToList();

        List<BO.TaskInList>? dependencies = (from item in dependentOn
                                             let alias = _dal.Task.Read(p => p.Id == item)?.Ailas ?? " "
                                             let description = _dal.Task.Read(p => p.Id == item)?.Description ?? " "
                                             let temp = (BO.Enums.Status)Read(item ?? 0).Status
                                             select new BO.TaskInList()
                                             {
                                                 Id = _dal.Dependency.Read(t => t.Id == item)?.DependentOnTask ?? 0,
                                                 Alias = alias,
                                                 Description = description,
                                                 Status = temp
                                             }).ToList();


        DateTime? forecastDate = null;
        if (doTask.ScheduledDate != null && doTask.RequiredEffortTime != null)
        {
            forecastDate = doTask.StartDate > doTask.ScheduledDate ? doTask.StartDate : doTask.ScheduledDate;
            forecastDate = (forecastDate + doTask.RequiredEffortTime);
        }


        var engineer = _dal.Task.Read(p => p.Id == id);
        BO.EngineerInTask? engineerInTask = null;
        if (engineer != null)
        {
            engineerInTask = new BO.EngineerInTask()
            {
                Id = engineer.EngineerId.HasValue ? (int)engineer.EngineerId : 0,
                Name = engineer.EngineerId.HasValue ? _dal.Engineer.Read(p => p.Id == engineer.EngineerId)?.Name : " "
            };
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
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = engineerInTask,
            Copmlexity = (BO.Enums.EngineerExperience)doTask.Copmlexity
        };
    }

    /// <summary>
    /// Returns all entitys from the list that meets the condition
    /// </summary>
    /// <param name="filter">condition</param>
    /// <returns>Returns all entitys from the list that meets the condition</returns>
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null!)
    {
        var tempList = _dal.Task.ReadAll();
        var doTaskList = (from DO.Task doTask in tempList
                          select Read(doTask.Id)).ToList();

        if (filter != null)
            return (from item in doTaskList
                    where filter(item)
                    select item).ToList();

        return doTaskList;
    }

    /// <summary>
    /// Update of an existing object
    /// </summary>
    /// <param name="item">An object of type Bo</param>
    /// <exception cref="BO.BlCannotUpdateWrongStateException">Cannot Update WrongS tate</exception>
    /// <exception cref="BO.BlNoDateException">No Date</exception>
    /// <exception cref="BO.BlDateClashException">Date Clash</exception>
    /// <exception cref="BO.BlCannotBeDeletedException">Cannot Be Deleted</exception>
    public void Update(BO.Task item)
    {

        var boTask = Read(item.Id);
        // if (doTask == null) throw new BO.BlDoesNotExistException(item.Id, _entityName);

        if (state.StatusProject() == Enums.ProjectStatus.Creation)
        {
            if (item.StartDate != boTask.StartDate ||
                       item.ScheduledDate != boTask.ScheduledDate ||

                       item.CompleteDate != boTask.CompleteDate ||
                       item.Engineer != null)
                throw new BO.BlCannotUpdateWrongStateException("There is a field that must not be changed at this stage");
        }

        if (state.StatusProject() == Enums.ProjectStatus.Scheduling)
        {
            if (item.CreatedAtDate != boTask.CreatedAtDate ||
                       item.RequiredEffortTime != boTask.RequiredEffortTime ||
                       item.StartDate != boTask.StartDate ||
                       item.CompleteDate != boTask.CompleteDate ||
                       item.Engineer != null)
                throw new BO.BlCannotUpdateWrongStateException("There is a field that must not be changed at this stage");
        }

        if (state.StatusProject() == Enums.ProjectStatus.Start)
        {
            if (item.Description != boTask.Description ||
                 item.CreatedAtDate != boTask.CreatedAtDate ||
                  item.RequiredEffortTime != boTask.RequiredEffortTime ||
                  item.ScheduledDate != boTask.ScheduledDate ||
                item.Dependencies != boTask.Dependencies)
                throw new BO.BlCannotUpdateWrongStateException("There is a field that must not be changed at this stage");
            if (item.Description != boTask.Description)
                throw new BO.BlCannotUpdateWrongStateException("There is a field that must not be changed at this stage");
            if (item.CreatedAtDate != boTask.CreatedAtDate)
                throw new BO.BlCannotUpdateWrongStateException("There is a field that must not be changed at this stage");
            if (item.RequiredEffortTime != boTask.RequiredEffortTime)
                throw new BO.BlCannotUpdateWrongStateException("There is a field that must not be changed at this stage");
            if (item.ScheduledDate != boTask.ScheduledDate)
                throw new BO.BlCannotUpdateWrongStateException("There is a field that must not be changed at this stage");
            if (item.Dependencies != boTask.Dependencies)
                throw new BO.BlCannotUpdateWrongStateException("There is a field that must not be changed at this stage");
        }

        if (item.RequiredEffortTime != boTask.RequiredEffortTime)

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

        DO.Task updatedTask = new DO.Task
        (item.Id, item.Alias, item.Description, item.CreatedAtDate, item.StartDate
        , item.ScheduledDate, item.ForecastDate, item.CompleteDate, item.RequiredEffortTime,
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

    /// <summary>
    /// updates a date of a task
    /// </summary>
    /// <param name="id">id of entity to update</param>
    /// <param name="date">new date</param>
    /// <exception cref="BO.BlNoDateException">No Date</exception>
    /// <exception cref="BO.BlDateClashException">Date Clash</exception>
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
            DO.Task newTask = _dal.Task.Read(p => p.Id == id)! with { ScheduledDate = date, DeadlineDate = (date + boTask.RequiredEffortTime) };
            _dal.Task.Update(newTask);

        }
    }

    public void AutoScheduling()
    {
        if (state.StartProject == null || state.EndProject == null)
            throw new BlCannotDoAutoSchedulingException("Project dates were not enterd\n");
        var tasks = ReadAll();
        foreach (var task in tasks)
        {
            if (task.StartDate == null)
                setAutoDate(task);
        }
    }

    private void setAutoDate(BO.Task task)
    {
        DateTime? max = state.StartProject;
        if (task.Dependencies != null)
            foreach (var dep in task.Dependencies)
            {
                try
                {
                    var depTask = Read(dep.Id);
                    if (depTask.ForecastDate == null)
                    {
                        setAutoDate(depTask);
                        max = max > depTask.ForecastDate ? max : depTask.ForecastDate;
                    }
                }
                catch (Exception ex)
                { throw new BlDoesNotExistException(ex); }
            }
        UpdateDate(task.Id, (DateTime)max!);
    }
}

