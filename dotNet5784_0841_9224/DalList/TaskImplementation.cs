﻿
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
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
    /// <exception cref="NotImplementedException">An attempt to delete an object that does not exist</exception>
    public void Delete(int id)
    {
        if (DataSource.Tasks.Find(d => d.Id == id) == null)
        {
            throw new NotImplementedException($"Task with ID={id} does Not exist");
        }
        DataSource.Tasks.Remove(DataSource.Tasks.Find(d => d.Id == id)!);
    }
    /// <summary>
    /// Returning a reference to a single object of type Task with a certain ID, if it exists in
    /// a database (in a list of data of type Task), or null if the object does not exist.
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <returns>If there is an object in the database with the received ID number, the method
    /// will return a reference to the existing object.Otherwise, the method will return null.</returns>
    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(d => d.Id == id);
    }
    /// <summary>
    /// Return a copy of the list of references to all objects of type Task
    /// </summary>
    /// <returns>The method returns a new list that is a copy of the existing list of all objects of type Task.</returns>
    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }
    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with 
    /// a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item">A reference to an updated existing object of type Dependency</param>
    /// <exception cref="NotImplementedException">In case there is no object with the received id</exception>
    public void Update(Task item)
    {
        if (DataSource.Tasks.Find(d => d.Id == item.Id) == null)
        {
            throw new NotImplementedException($"Task with ID={item.Id} does Not exist");
        }
       
        Delete((DataSource.Tasks.Find(d => d.Id == item.Id)).Id);
        DataSource.Tasks.Add(item);
        return;
    }
}