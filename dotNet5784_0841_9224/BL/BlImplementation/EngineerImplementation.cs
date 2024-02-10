namespace BlImplementation;
using BlApi;
using DalApi;
using System;
using System.Collections.Generic;


internal class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private const string _entityName = nameof(BO.Engineer);
    private BO.Engineer? BOFromDO(DO.Engineer doEngineer)
    {
        return new BO.Engineer()
        {

            Id = doEngineer.Id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.Enums.EngineerExperience)doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = new BO.TaskInEngineer()
            {
                Id = _dal.Task.Read(p => p.EngineerId == doEngineer.Id)!.Id,
                Alias = _dal.Task.Read(p => p.EngineerId == doEngineer.Id)!.Ailas
            }
        };
    }

    public void Create(BO.Engineer item)
    {
        if (item.Id <= 0) throw new BO.BlTheInputIsInvalidException("Id");
        if (item.Name == null) throw new BO.BlTheInputIsInvalidException("Name");
        if (item.Cost == null || item.Cost <= 0) throw new BO.BlTheInputIsInvalidException("Cost");
        if (item.Email == null) throw new BO.BlTheInputIsInvalidException("Email");
        if (item.Email != null)
        {
            if (!item.Email.Contains("@")) throw new BO.BlTheInputIsInvalidException("Email");
            if (!item.Email.Contains(".")) throw new BO.BlTheInputIsInvalidException("Email");
        }

        DO.Engineer doEngineer = new DO.Engineer
            (item.Id, item.Name, item.Email, (DO.EngineerExperience)item.Level, item.Cost);

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
        if (id == 0)
        {
            _dal.Engineer.DeleteAll();
            return;
        }

        BO.Engineer engineer = Read(id)!;

        var doTask = _dal.Task.ReadAll().Where(p => p.EngineerId == id).ToList();
        if (doTask == null)
        {
            try
            {
                _dal.Engineer.Delete(id);
                return;
            }
            catch (DO.DalDoesNotExistsException ex)
            {
                throw new BO.BlCannotBeDeletedException(id, _entityName, ex);
            }
        }
        throw new BO.BlCannotBeDeletedException(id, _entityName);
    }

    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(p => p.Id == id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException(id, _entityName);

        BO.TaskInEngineer taskInEngineer = null!;

        DO.Task? temp = _dal.Task.Read(p => p.EngineerId == doEngineer.Id);
        if (temp != null)
            taskInEngineer = new BO.TaskInEngineer() { Id = temp.Id, Alias = temp.Ailas };

        return BOFromDO(doEngineer);
    }

    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        var doEngineerList = (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                              select BOFromDO( doEngineer)).ToList();
        if (filter != null)
            return (from item in doEngineerList
                    where filter(item)
                    orderby item.Cost
                    select item).ToList();

        return doEngineerList;
    }

    public void Update(BO.Engineer item)
    {

        var doEngeenir = _dal.Engineer.Read(p => p.Id == item.Id);
        if (doEngeenir == null) throw new BO.BlDoesNotExistException(item.Id, _entityName);
        if (item.Name == null) throw new BO.BlTheInputIsInvalidException("Name");
        if (item.Cost <= 0) throw new BO.BlTheInputIsInvalidException("Cost");
        if (item.Email != null)
        {
            if (!item.Email.Contains("@")) throw new BO.BlTheInputIsInvalidException("Email");
            if (!item.Email.Contains(".")) throw new BO.BlTheInputIsInvalidException("Email");
        }
        if ((doEngeenir.Level > (DO.EngineerExperience)item.Level)) throw new BO.BlTheInputIsInvalidException("Level");
        try
        {
            DO.Engineer updatedEngeenir = new DO.Engineer(item.Id, item.Name, item.Email, (DO.EngineerExperience)item.Level, item.Cost);
            _dal.Engineer.Update(updatedEngeenir);

            if (item.Task != null)
            {
                var doTask = _dal.Task.Read((p => p.Id == item.Task.Id));
                if (doTask != null)
                {
                    DO.Task updatedTask = doTask with { EngineerId = item.Id };
                    _dal.Task.Update(updatedTask);
                }
            }
        }
        catch (DO.DalDoesNotExistsException ex)
        {
            throw new BO.BlDoesNotExistException(ex);
        }

    }
    public IEnumerable<BO.Engineer> ReadAllDelete()
    {
        var templist = _dal.Engineer.ReadAllDelete();
        return from item in templist
               select BOFromDO(item);
    }
    public void DeleteAll()
    {
        _dal.Engineer.DeleteAll();
    }
}
