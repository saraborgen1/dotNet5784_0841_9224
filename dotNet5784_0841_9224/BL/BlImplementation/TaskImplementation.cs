namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using System;
using System.Collections.Generic;

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
            throw new BO.BlAlreadyExistsException($"Engineer with ID={item.Id} already exists", ex);
        }

    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(p => p.Id == id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        return new BO.Task()
        {
            Id = doTask.Id,
            Alias = doTask.Ailas,
            Description = doTask.Description,
            CreatedAtDate = doTask.CreatedAtDate,
            //Status
            //Dependencies= doTask.
            //Milestone
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            //ForecastDate= doTask.
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            ///*Engineer*/
            //if (doTask.Copmlexity != null) 
            Copmlexity = (BO.Enums.EngineerExperience)(int)doTask.Copmlexity
        };
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool> filter = null!)
    {
        var doTaskList = (from DO.Task doTask in _dal.Task.ReadAll()
                          select new BO.Task()
                          {
                              Id = doTask.Id,
                              Alias = doTask.Ailas,
                              Description = doTask.Description,
                              CreatedAtDate = doTask.CreatedAtDate,
                              //Status
                              //Dependencies= doTask.
                              //Milestone
                              RequiredEffortTime = doTask.RequiredEffortTime,
                              StartDate = doTask.StartDate,
                              ScheduledDate = doTask.ScheduledDate,
                              //ForecastDate= doTask.
                              DeadlineDate = doTask.DeadlineDate,
                              CompleteDate = doTask.CompleteDate,
                              Deliverables = doTask.Deliverables,
                              Remarks = doTask.Remarks,
                              ///*Engineer*/
                              //if (doTask.Copmlexity != null) 
                              Copmlexity = (BO.Enums.EngineerExperience)(int)doTask.Copmlexity
                          }).ToList();

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
        throw new NotImplementedException();
    }
}
