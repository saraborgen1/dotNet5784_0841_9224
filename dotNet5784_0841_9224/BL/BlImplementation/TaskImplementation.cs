namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(Task item)
    {
        if (item.Id <= 0) throw new ArgumentException();
        if (item.Alias == " ") throw new ArgumentException();
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
        throw new NotImplementedException();
    }

    public void UpdateDate(int id, DateTime date)
    {
        throw new NotImplementedException();
    }
}
