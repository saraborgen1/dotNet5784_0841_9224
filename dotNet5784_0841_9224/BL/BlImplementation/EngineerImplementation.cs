namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Implementation of the IEngineer interface which handles operations related to engineers.
/// </summary>
internal class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private const string _entityName = nameof(BO.Engineer);
    private static readonly Random s_rand = new();

    /// <summary>
    /// Converts a DO Engineer entity to a BO Engineer entity.
    /// </summary>
    private BO.Engineer BOFromDO(DO.Engineer doEngineer)
    {
        return new BO.Engineer()
        {
            Id = doEngineer.Id,
            Password = doEngineer.Password,
            Salt = doEngineer.Salt,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.Enums.EngineerExperience)doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = getTaskInEngineer(doEngineer.Id)
        };
    }

    /// <summary>
    /// Adds a new engineer to the database.
    /// </summary>
    public void Create(BO.Engineer item)
    {
        // Input validation
        if (item.Id <= 0) throw new BO.BlTheInputIsInvalidException("Id");
        if (item.Name == null) throw new BO.BlTheInputIsInvalidException("Name");
        if (item.Cost == null || item.Cost <= 0) throw new BO.BlTheInputIsInvalidException("Cost");
        if (item.Email == null) throw new BO.BlTheInputIsInvalidException("Email");
        if (item.Email != null)
        {
            if (!item.Email.Contains("@")) throw new BO.BlTheInputIsInvalidException("Email");
            if (!item.Email.Contains(".")) throw new BO.BlTheInputIsInvalidException("Email");
            foreach (char c in item.Email)
            {
                if (char.IsWhiteSpace(c))
                {
                    throw new BO.BlTheInputIsInvalidException("Email");
                }
            }
        }
        if (item.Password == null || item.Password.Length < 8 || item.Password.Length > 13) throw new BO.BlTheInputIsInvalidException("Password");

        // Generate salt for password hashing
        item.Salt = s_rand.Next();
        // Create DO Engineer object with hashed password and add it to the database
        DO.Engineer doEngineer = new DO.Engineer(item.Id, hashPassword(item.Password + item.Salt), item.Salt, item.Name, item.Email, (DO.EngineerExperience)item.Level, item.Cost);

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
    /// Deletes an engineer with the specified ID from the database.
    /// </summary>
    public void Delete(int id)
    {
        // Checks if engineer is in the middle of a mission
        BO.Engineer engineer = Read(id)!;
        if (engineer.Task == null)
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
        throw new BO.BlCannotBeDeletedException(id, _entityName, "in the middle of a mission");
    }

    /// <summary>
    /// Retrieves an engineer with the specified ID from the database.
    /// </summary>
    public BO.Engineer? Read(int id)
    {
        // Retrieves engineer from the database
        DO.Engineer? doEngineer = _dal.Engineer.Read(p => p.Id == id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException(id, _entityName);

        // Retrieves associated task
        BO.TaskInEngineer taskInEngineer = null!;
        DO.Task? temp = _dal.Task.Read(p => p.EngineerId == doEngineer.Id);
        if (temp != null)
            taskInEngineer = new BO.TaskInEngineer() { Id = temp.Id, Alias = temp.Ailas };

        // Converts DO Engineer to BO Engineer and returns
        return BOFromDO(doEngineer);
    }

    /// <summary>
    /// Retrieves all engineers from the database.
    /// </summary>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        // Retrieves all engineers from the database
        var doEngineerList = (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                              select BOFromDO(doEngineer)).ToList();
        if (filter != null)
            return (from item in doEngineerList
                    where filter(item)
                    orderby item.Cost
                    select item).ToList();

        return doEngineerList;
    }

    /// <summary>
    /// Updates an existing engineer in the database.
    /// </summary>
    public void Update(BO.Engineer item)
    {
        // Retrieves engineer from the database
        var doEngeenir = _dal.Engineer.Read(p => p.Id == item.Id);
        if (doEngeenir == null) throw new BO.BlDoesNotExistException(item.Id, _entityName);

        // Input validation
        if (item.Name == null) throw new BO.BlTheInputIsInvalidException("Name");
        if (item.Cost <= 0) throw new BO.BlTheInputIsInvalidException("Cost");
        if (item.Email != null)
        {
            if (!item.Email.Contains("@")) throw new BO.BlTheInputIsInvalidException("Email");
            if (!item.Email.Contains(".")) throw new BO.BlTheInputIsInvalidException("Email");
            foreach (char c in item.Email)
            {
                if (char.IsWhiteSpace(c))
                {
                    throw new BO.BlTheInputIsInvalidException("Email");
                }
            }
        }
        if ((doEngeenir.Level > (DO.EngineerExperience)item.Level)) throw new BO.BlTheInputIsInvalidException("Level");

        try
        {
            // Update engineer details
            DO.Engineer updatedEngeenir = new DO.Engineer(item.Id, item.Password, item.Salt, item.Name, item.Email, (DO.EngineerExperience)item.Level, item.Cost);
            _dal.Engineer.Update(updatedEngeenir);

            // Update associated task if provided
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
    /// Retrieves all deleted engineers from the database.
    /// </summary>
    public IEnumerable<BO.Engineer> ReadAllDelete()
    {
        // Retrieves all deleted engineers from the database
        var templist = _dal.Engineer.ReadAllDelete();
        return from item in templist
               select BOFromDO(item);
    }

    /// <summary>
    /// Deletes all engineers from the database.
    /// </summary>
    public void DeleteAll()
    {
        // Deletes all engineers from the database
        _dal.Engineer.DeleteAll();
    }

    /// <summary>
    /// Retrieves the password of the engineer with the specified ID.
    /// </summary>
    public string GetPassword(int id)
    {
        // Retrieves and returns the password of the engineer with the specified ID
        return Read(id)!.Password!;
    }

    /// <summary>
    /// Compares the provided password with the stored hashed password of the engineer with the specified ID.
    /// </summary>
    public bool comparePassword(int id, string password)
    {
        // Compares the provided password with the stored hashed password of the engineer with the specified ID
        // Returns true if passwords match, false otherwise
        var temp = Read(id);
        if (temp!.Password == hashPassword(password + temp.Salt))
            return true;
        return false;
    }

    /// <summary>
    /// Restores a deleted engineer in the database.
    /// </summary>
    public void RestoreEngineer(BO.Engineer engineer)
    {
        // Restores a deleted engineer in the database if it exists
        // Throws exception if engineer does not exist
        var temp = _dal.Engineer.ReadAllDelete().FirstOrDefault(t => t.Id == engineer.Id);
        if (temp != null)
        {
            temp.Active = true;
            try
            {
                _dal.Engineer.Update(temp);
            }
            catch (DO.DalDoesNotExistsException ex)
            {
                throw new BO.BlDoesNotExistException(ex);
            }
        }
    }

    /// <summary>
    /// Retrieves the task assigned to the engineer with the specified ID.
    /// </summary>
    private TaskInEngineer? getTaskInEngineer(int id)
    {
        // Retrieves the task assigned to the engineer with the specified ID from the database
        // Converts DO Task to BO TaskInEngineer and returns
        var task = _dal.Task.Read(p => (p.EngineerId == id && p.CompleteDate == null));
        if (task != null)
            return new BO.TaskInEngineer()
            {
                Id = task.Id,
                Alias = task.Ailas
            };
        return null;
    }

    /// <summary>
    /// Retrieves available tasks that can be assigned to the engineer.
    /// </summary>
    public List<BO.Task> AvailableTasks(BO.Engineer engineer)
    {
        // Retrieves available tasks that match engineer's level and dependencies are completed
        // Converts DO Tasks to BO Tasks and returns
        var _task = new TaskImplementation();
        return _task.ReadAll(s => s.Engineer?.Id == null && s.Copmlexity <= engineer.Level && AreDependentTasksCompleted(s)).ToList();
    }

    /// <summary>
    /// Checks if all dependent tasks of a given task are completed.
    /// </summary>
    private bool AreDependentTasksCompleted(BO.Task task)
    {
        // Checks if all dependent tasks of a given task are completed
        // Returns true if all dependencies are completed, false otherwise
        var _task = new TaskImplementation();
        if (task.Dependencies == null || task.Dependencies.Count == 0)
            return true;

        foreach (var dependentTask in task.Dependencies)
        {
            if (dependentTask.Status != BO.Enums.Status.Done)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Hashes the provided password with salt using SHA512.
    /// </summary>
    private static string hashPassword(string passwordWithSalt)
    {
        // Hashes the provided password with salt using SHA512 algorithm
        SHA512 shaM = new SHA512Managed();
        return Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt)));
    }
}
