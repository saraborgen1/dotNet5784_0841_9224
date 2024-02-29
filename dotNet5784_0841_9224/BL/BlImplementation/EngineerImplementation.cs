namespace BlImplementation;
using BlApi;
using DalApi;
using System;
using System.Collections.Generic;


internal class EngineerImplementation : BlApi.IEngineer
{
   
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private const string _entityName = nameof(BO.Engineer);

    /// <summary>
    /// converts entity of DO to BO
    /// </summary>
    /// <param name="doEngineer"> what is being converted</param>
    /// <returns> entity of  BO type</returns>
    private BO.Engineer BOFromDO(DO.Engineer doEngineer)
    {
        return new BO.Engineer()
        {

            Id = doEngineer.Id,
            Password = doEngineer.Password,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.Enums.EngineerExperience)doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = new BO.TaskInEngineer()
            {
                Id = _dal.Task.Read(p => p.EngineerId == doEngineer.Id)!?.Id ??0,
                Alias = _dal.Task.Read(p => p.EngineerId == doEngineer.Id)!?.Ailas ??" "
            }
        };
    }

    /// <summary>
    /// Adding a new object to  database
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="BO.BlTheInputIsInvalidException">The Input Is Invalid</exception>
    /// <exception cref="BO.BlAlreadyExistException">Already Exist</exception>
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
            (item.Id,item.Password, item.Name, item.Email, (DO.EngineerExperience)item.Level, item.Cost);

        try
        {
            int idEng = _dal.Engineer.Create(doEngineer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistException(ex);
        }
    }

    /// <summary>
    /// Deletion of an existing object with a certain ID, from the list of entity type objects.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlCannotBeDeletedException">Cannot Be Deleted</exception>
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

    /// <summary>
    /// Returns an entity from the list that id matches param
    /// </summary>
    /// <param name="id"></param>
    /// <returns>an entity from the list that meets the condition</returns>
    /// <exception cref="BO.BlDoesNotExistException">Does Not Exist</exception>
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

    /// <summary>
    /// Returns all entitys from the list that meets the condition
    /// </summary>
    /// <param name="filter">condition</param>
    /// <returns>Returns all entitys from the list that meets the condition</returns>
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

    /// <summary>
    /// Update of an existing object
    /// </summary>
    /// <param name="item">An object of type Bo</param>
    /// <exception cref="BO.BlDoesNotExistException">Does Not Exist</exception>
    /// <exception cref="BO.BlTheInputIsInvalidException">The Input Is Invalid</exception>
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
            DO.Engineer updatedEngeenir = new DO.Engineer(item.Id,item.Password,item.Name, item.Email, (DO.EngineerExperience)item.Level, item.Cost);
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

    /// <summary>
    /// returns all deleted engineer
    /// </summary>
    /// <returns>all deleted engineer</returns>
    public IEnumerable<BO.Engineer> ReadAllDelete()
    {
        var templist = _dal.Engineer.ReadAllDelete();
        return from item in templist
               select BOFromDO(item);
    }

    /// <summary>
    /// deletes all objects of the entity type
    /// </summary>
    public void DeleteAll()
    {
        _dal.Engineer.DeleteAll();
    }

    /// <summary>
    /// returns password of engineer with that id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>password</returns>
    public int GetPassword(int id)
    {
        return (int)Read(id)!.Password!;
    }
}
