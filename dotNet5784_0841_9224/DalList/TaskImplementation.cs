namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    /// <summary>
    /// Adding a new object of type Task to a database, (to the list of objects of type Task).
    /// </summary>
    /// <param name="item">A reference to an existing object of type Task</param>
    /// <returns>The method will return the running number of the newly created object in the list</returns>
    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task newTask = item with { Id = newId };
        DataSource.Tasks.Add(newTask);
        return newId;
    }

    /// <summary>
    /// Deletion of an existing object with a certain ID, from the list of objects of type Task
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <exception cref="DalDoesNotExistException">An attempt to delete an object that does not exist</exception>
    public void Delete(int id)
    {
        if (DataSource.Tasks.FirstOrDefault(item => item.Id == id) == null)
            throw new DalDoesNotExistException($"Task with ID={id} does Not exist");

        DataSource.Tasks.Remove(DataSource.Tasks.FirstOrDefault(item => item.Id == id)!);
    }

    /// <summary>
    /// Returns an entity from the list that meets the condition
    /// </summary>
    /// <param name="filter">condition</param>
    /// <returns>Returns an entity from the list that meets the condition</returns>
    public Task? Read(Func<Task, bool> filter)
     => DataSource.Tasks.FirstOrDefault(filter);

    /// <summary>
    /// Returns a collection of entities that meet the condition
    /// </summary>
    /// <param name="filter">some condition</param>
    /// <returns>A collection of entities that meet the condition</returns>
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null) //stage 2
    {
        if (filter != null)
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;

        return from item in DataSource.Tasks
               select item;
    }

    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with 
    /// a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item">A reference to an updated existing object of type Dependency</param>
    public void Update(Task item)
    {
        Delete(item.Id);

        DataSource.Tasks.Add(item);
    }
}
