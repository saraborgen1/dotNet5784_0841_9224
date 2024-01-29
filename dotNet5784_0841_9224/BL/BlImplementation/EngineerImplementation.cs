namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;


internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private const string _entityName = nameof(BO.Engineer);
    public void Create(BO.Engineer item)
    {
        if (item.Id <= 0) throw new BlTheInputIsInvalidException("Id");
        if (item.Name == null) throw new BlTheInputIsInvalidException("Name");
        if (item.Cost <= 0) throw new BlTheInputIsInvalidException("Cost");
        if (item.Email != null)
        {
            if (!item.Email.Contains("@")) throw new BlTheInputIsInvalidException("Email");
            if (!item.Email.Contains(".")) throw new BlTheInputIsInvalidException("Email");
        }

        DO.Engineer doEngineer = new DO.Engineer
            (item.Id, item.Name, item.Email, item.Level, item.Cost);

        try
        {
            int idEng = _dal.Engineer.Create(doEngineer);
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
            BO.Engineer engineer = Read(id)!;
            if ((engineer.Task == null) || (BO.Task.Read(engineer.Task.Id).StartDate == null) || (BO.Task.Read(engineer.Task.Id).StartDate > DateTime.Now))
            {
                _dal.Engineer.Delete(id);
                return;
            }
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlCannotBeDeletedException(id, _entityName, ex);
        }

    }

    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(p => p.Id == id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException(id, _entityName);

        BO.TaskInEngineer taskInEngineer = null!;

        DO.Task? temp = _dal.Task.Read(p => p.EngineerId == doEngineer.Id);
        if (temp != null)
            taskInEngineer = new TaskInEngineer() { Id = temp.Id, Alias = temp.Ailas };

        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = taskInEngineer
        };
    }

    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool> filter = null!)
    {
        var doEngineerList = (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                              select new BO.Engineer()
                              {

                                  Id = doEngineer.Id,
                                  Name = doEngineer.Name,
                                  Email = doEngineer.Email,
                                  Level = doEngineer.Level,
                                  Cost = doEngineer.Cost,
                                  Task = new TaskInEngineer()
                                  {
                                      Id = _dal.Task.Read(p => p.EngineerId == doEngineer.Id)!.Id,
                                      Alias = _dal.Task.Read(p => p.EngineerId == doEngineer.Id)!.Ailas
                                  }
                              }).ToList();

        if (filter != null)
            return (from item in doEngineerList
                    where filter(item)
                    select item).ToList();

        return doEngineerList;
    }

    public void Update(BO.Engineer item)
    {

        var doEngeenir = _dal.Engineer.Read(p => p.Id == item.Id);
        if (item.Name == null) throw new BlTheInputIsInvalidException("Name");
        if (item.Cost <= 0) throw new BlTheInputIsInvalidException("Cost");
        if (item.Email != null)
        {
            if (!item.Email.Contains("@")) throw new BlTheInputIsInvalidException("Email");
            if (!item.Email.Contains(".")) throw new BlTheInputIsInvalidException("Email");
        }
        if ((doEngeenir!.Level != null) && (doEngeenir.Level > item.Level)) throw new BlTheInputIsInvalidException("Level");
        try
        {
            DO.Engineer updatedEngeenir = new DO.Engineer(item.Id, item.Name, item.Email, item.Level, item.Cost);
            _dal.Engineer.Update(updatedEngeenir);

            if (item.Task != null)
            {

                var doTask = _dal.Task.Read((p => p.Id == item.Task.Id));
                DO.Task updatedTask = doTask with { EngineerId = item.Task.Id };
                _dal.Task.Update(updatedTask);
            }
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistException(ex);
        }

    }
}
