namespace BlImplementation;
using BlApi;
using BO;
using System.Data;
using System.Linq;
using static BO.Enums;

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
        if (state.StatusProject() != BO.Enums.ProjectStatus.Creation)
            throw new BO.BlCannotAddWrongStateException("Cannot create a task at this state");
        if (state.StatusProject() != BO.Enums.ProjectStatus.Creation && item.Id <= 0) throw new BO.BlTheInputIsInvalidException("Id");
        if (item.Alias == null) throw new BO.BlTheInputIsInvalidException("Name");
        if (item.Description == null) throw new BO.BlTheInputIsInvalidException("Description");
        if (item.RequiredEffortTime == null || item.RequiredEffortTime < TimeSpan.Zero) throw new BO.BlTheInputIsInvalidException("RequiredEffortTime");
        if (item.Deliverables == null) throw new BO.BlTheInputIsInvalidException("Deliverables");
        if (item.StartDate != null)
            throw new BO.BlCannotUpdateWrongStateException("StartDate", "Creation");
        if (item.ScheduledDate != null)
            throw new BO.BlCannotUpdateWrongStateException("ScheduledDate", "Creation");
        if (item.CompleteDate !=null)
            throw new BO.BlCannotUpdateWrongStateException("CompleteDate", "Creation");
        if (item.Engineer?.Id != null || item.Engineer?.Name != null)
            throw new BO.BlCannotUpdateWrongStateException("Engineer", "Creation");
        if (item.ForecastDate != null)
            throw new BO.BlCannotUpdateWrongStateException("ForecastDate", "Creation");
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
            var boTask = Read(idTask);
            updateDependencies(item, boTask);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistException(ex);
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistException(ex);
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
        if (dependenciesId != null && dependenciesId.Count!=0)
            throw new BO.BlCannotBeDeletedException(id, _entityName ,"has tasks dependent");

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

        List<BO.TaskInList>? dependencies = GetAllTaskInList(id);


        DateTime? forecastDate = null;
        if (doTask.ScheduledDate != null && doTask.RequiredEffortTime != null)
        {
            if (doTask.StartDate == null)
                forecastDate = doTask.ScheduledDate + doTask.RequiredEffortTime;
            else
            {
                forecastDate = doTask.StartDate > doTask.ScheduledDate ? doTask.StartDate : doTask.ScheduledDate;
                forecastDate = (forecastDate + doTask.RequiredEffortTime);
            }
        }


        var engineer = _dal.Task.Read(p => p.Id == id);

        BO.EngineerInTask? engineerInTask = null;
        if (engineer != null)
        {
            engineerInTask = new BO.EngineerInTask()
            {
                Id = engineer.EngineerId.HasValue ? engineer.EngineerId : null,
                Name = engineer.EngineerId.HasValue ? _dal.Engineer.Read(p => p.Id == engineer.EngineerId)?.Name : " "
            };
            if (engineerInTask.Id == null)
                engineerInTask = new BO.EngineerInTask();
        }


        BO.Enums.Status status = getStatus(doTask);


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

        if (state.StatusProject() == Enums.ProjectStatus.Creation)
        {
            if (item.StartDate != boTask.StartDate)
                throw new BO.BlCannotUpdateWrongStateException("StartDate", "Creation");
            if (item.ScheduledDate != boTask.ScheduledDate)
                throw new BO.BlCannotUpdateWrongStateException("ScheduledDate", "Creation");
            if (item.CompleteDate != boTask.CompleteDate)
                throw new BO.BlCannotUpdateWrongStateException("CompleteDate", "Creation");
            if (item.Engineer?.Id != null|| item.Engineer?.Name!=null)
                throw new BO.BlCannotUpdateWrongStateException("Engineer", "Creation");
            if(item.ForecastDate!= boTask.ForecastDate)
                throw new BO.BlCannotUpdateWrongStateException("ForecastDate", "Creation");
        }

        if (state.StatusProject() == Enums.ProjectStatus.Scheduling)
        {
            if (item.CreatedAtDate != boTask.CreatedAtDate)
                throw new BO.BlCannotUpdateWrongStateException("CreatedAtDate", "Scheduling");
            if (item.RequiredEffortTime != boTask.RequiredEffortTime)
                throw new BO.BlCannotUpdateWrongStateException("RequiredEffortTime", "Scheduling");
            if (item.StartDate != boTask.StartDate)
                throw new BO.BlCannotUpdateWrongStateException("StartDate", "Scheduling");
            if (item.CompleteDate != boTask.CompleteDate)
                throw new BO.BlCannotUpdateWrongStateException("CompleteDate", "Scheduling");
            if (item.Engineer?.Id != null && item.Engineer?.Name!=null)
                throw new BO.BlCannotUpdateWrongStateException("Engineer", "Scheduling");
            if (item.ForecastDate != boTask.ForecastDate)
                throw new BO.BlCannotUpdateWrongStateException("ForecastDate", "Scheduling");
        }

        if (state.StatusProject() == Enums.ProjectStatus.Start)
        {
            if (item.CreatedAtDate != boTask.CreatedAtDate)
                throw new BO.BlCannotUpdateWrongStateException("CreatedAtDate", "Start");
            if (item.RequiredEffortTime != boTask.RequiredEffortTime)
                throw new BO.BlCannotUpdateWrongStateException("RequiredEffortTime", "Start");
           if(item.ScheduledDate != boTask.ScheduledDate)
                throw new BO.BlCannotUpdateWrongStateException("ScheduledDate", "Start");
            if (item.ForecastDate != boTask.ForecastDate)
                throw new BO.BlCannotUpdateWrongStateException("ForecastDate", "Start");
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
        if (boTask.Engineer == null || item.Engineer.Id != boTask.Engineer.Id)
        {
            if (item.Engineer.Id != null)
            {
                var engineer = _dal.Engineer.Read(p => p.Id == item.Engineer.Id);
                if (engineer == null)
                    throw new BO.BlDateClashException("An engineer with this ID does not exist");

                if (item.Engineer.Name != engineer.Name)
                    throw new BO.BlDateClashException("This name does not match the name of the holder of this ID");
            }

        }
        updateDependencies(item, boTask);

        DO.Task updatedTask = new DO.Task
        (item.Id, item.Alias, item.Description, item.CreatedAtDate, item.StartDate
        , item.ScheduledDate, item.ForecastDate, item.CompleteDate, item.RequiredEffortTime,
         item.Deliverables, item.Remarks, item.Engineer?.Id ?? null, (DO.EngineerExperience)item.Copmlexity);

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
        if (dependencies!.Count != 0)
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
        }
        DO.Task newTask = _dal.Task.Read(p => p.Id == id)! with { ScheduledDate = date, DeadlineDate = (date + boTask.RequiredEffortTime) };
        _dal.Task.Update(newTask);
    }
    /// <summary>
    /// auto schedualing
    /// </summary>
    /// <exception cref="BlCannotDoAutoSchedulingException"></exception>
    public void AutoScheduling()
    {
        if (state.StatusProject() == Enums.ProjectStatus.Creation)
            throw new BlCannotDoAutoSchedulingException("Project dates were not enterd\n");
        var tasks = ReadAll();
        foreach (var task in tasks)
        {
            var tempTask = Read(task.Id);
            if (tempTask.Status == Enums.Status.Unscheduled)
                setAutoDate(tempTask);
        }
    }
    /// <summary>
    /// auto date
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BlDoesNotExistException"></exception>
    private void setAutoDate(BO.Task task)
    {
        DateTime? max = state.StartProject;
        if (task.Dependencies!.Count != 0)
            foreach (var dep in task.Dependencies)
            {
                try
                {
                    var depTask = Read(dep.Id);
                    if (depTask.Status == Status.Unscheduled)
                        setAutoDate(depTask);

                    depTask = Read(dep.Id);
                    max = max > depTask.ForecastDate ? max : depTask.ForecastDate;
                }
                catch (Exception ex)
                { throw new BlDoesNotExistException(ex); }
            }
        UpdateDate(task.Id, (DateTime)max!);
    }
    /// <summary>
    /// returns task in list
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    public TaskInList GetTaskInList(int id)
    {
        var doTask = _dal.Task.Read(p => p.Id == id);
        if (doTask == null)
            throw new BlDoesNotExistException(id, _entityName);
        return new BO.TaskInList()
        {
            Id = doTask.Id,
            Alias = doTask.Ailas,
            Description = doTask.Description,
            Status = getStatus(doTask)
        };

    }
    /// <summary>
    /// returns all of the TaskInList
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    public List<TaskInList> GetAllTaskInList(int id)
    {
        var doTask = _dal.Task.Read(p => p.Id == id);
        if (doTask == null)
            throw new BlDoesNotExistException(id, _entityName);

        List<int> dependentOn = (from temp in _dal.Dependency.ReadAll(p => p.DependentTask == id)
                                 where temp.DependentOnTask != null
                                 select (int)temp.DependentOnTask!).ToList();

        return (from item in dependentOn
                select GetTaskInList(item)).OrderBy(p => p.Id).ToList();
    }
    /// <summary>
    /// returns task status
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>

    private BO.Enums.Status getStatus(DO.Task task)
    {
        if (task.ScheduledDate == null)
            return Status.Unscheduled;
        if (task.CompleteDate != null)
            return Status.Done;
        if (task.StartDate != null)
            return Status.OnTrack;
        return Status.Scheduled;
    }
    /// <summary>
    /// usless
    /// </summary>
    /// <param name="id"></param>
    private void grouping(int id)
    {
        var gDependenciesId = (from temp in _dal.Dependency.ReadAll(p => p.DependentTask == id)
                               group temp by temp.DependentTask % id into gs
                               select gs).ToList();

        var dependenciesId = (from temp in gDependenciesId
                              where temp.Key == 0
                              from item in temp
                              select item.Id).ToList();
    }
    /// <summary>
    /// updates the dep
    /// </summary>
    /// <param name="item"></param>
    /// <param name="boTask"></param>
    /// <exception cref="BO.BlCannotUpdateWrongStateException"></exception>
    /// <exception cref="BlCirculDepException"></exception>
    /// <exception cref="BlDoesNotExistException"></exception>
    /// <exception cref="BlCannotBeDeletedException"></exception>
    private void updateDependencies(BO.Task item, BO.Task boTask)
    {
        var addDep = item.Dependencies != null ? item.Dependencies : new List<TaskInList>();
        var deletDep = boTask.Dependencies != null ? boTask.Dependencies : new List<TaskInList>();
        if (item.Dependencies != null && boTask.Dependencies != null)
        {
            addDep = item.Dependencies.Except(boTask.Dependencies).ToList();
            deletDep = boTask.Dependencies.Except(item.Dependencies).ToList();
        }
        if (state.StatusProject() == Enums.ProjectStatus.Start)
            //  if (addDep.Count != 0 || deletDep.Count != 0)
            if (!CompareTaskDependencies(addDep, deletDep))
            {
                if (state.StatusProject() == Enums.ProjectStatus.Scheduling)
                    throw new BO.BlCannotUpdateWrongStateException("Dependencies", "Scheduling");
                if (state.StatusProject() == Enums.ProjectStatus.Start)
                    throw new BO.BlCannotUpdateWrongStateException("Dependencies", "Start");
            }
        foreach (var dep in addDep)
        {
           if(!circuleDep(boTask.Id, Read(dep.Id)))
                throw new BlCirculDepException("Cannot create a circular dependency");

            _dal.Dependency.Create(new DO.Dependency() { Id = 0, DependentOnTask = dep.Id, DependentTask = boTask.Id });
        }
        foreach (var dep in deletDep)
        {
            var temp = _dal.Dependency.Read(p => (p.DependentOnTask == dep.Id && p.DependentTask == boTask.Id));
            if (temp == null)
                throw new BlDoesNotExistException("There is no dependency with such details");
            try
            {
                _dal.Dependency.Delete(temp.Id);
            }
            catch (Exception ex)
            {
                throw new BlCannotBeDeletedException(ex);
            }

        }




    }
    /// <summary>
    /// checks if there are circle dep
    /// </summary>
    /// <param name="id"></param>
    /// <param name="boTask"></param>
    /// <returns></returns>
    public bool circuleDep(int id, BO.Task boTask)
    {
        if (boTask.Dependencies != null)
            foreach (var dep in boTask.Dependencies)
            {
                if (dep.Id == id)
                    return false;
                var task = Read(dep.Id);
                circuleDep(id, task);
            }
        return true;
    }
    /// <summary>
    /// checks if list of dep contain the same elements
    /// </summary>
    /// <param name="list1"></param>
    /// <param name="list2"></param>
    /// <returns>if they are the same </returns>
    private bool CompareTaskDependencies(List<TaskInList> list1, List<TaskInList> list2)
    {
        if (list1 == null && list2 == null)
            return true;
        if (list1 == null || list2 == null)
            return false;

        var idsFromList1 = list1.Select(task => task.Id).ToList();
        var idsFromList2 = list2.Select(task => task.Id).ToList();

        if (idsFromList1.SequenceEqual(idsFromList2))
            return true;
        return false;
    }
}


