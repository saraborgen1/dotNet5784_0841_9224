﻿namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// Adding a new object of type Engineer to a database, (to the list of objects of type Engineer).
    /// </summary>
    /// <param name="item">A reference to an existing object of the Engineer type. The object was created in an upper layer and its fields are already filled with normal values.</param>
    /// <returns>The method will return the running number of the newly created object in the list.</returns>
    /// <exception cref="NotImplementedException">In case of an attempt to add an object that already exists - an exception will be thrown</exception>
    public int Create(Engineer item)
    {

        if (DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == item.Id) != null)
        {
            throw new NotImplementedException($"Engineer with ID={item.Id} already exist");
        }
        DataSource.Engineers.Add(item);
        return item.Id;
    }
    /// <summary>
    /// Deletion of an existing object with a certain ID, from the list of Engineer type objects.
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <exception cref="NotImplementedException">If there is no object with the received ID number - an appropriate exception will be thrown</exception>
    public void Delete(int id)
    {
        if (DataSource.Tasks.FirstOrDefault(item => item.Id == id) == null)
        {
            throw new NotImplementedException($"Engineer with ID={id} does Not exist");
        }
        DataSource.Engineers.Remove(DataSource.Engineers.FirstOrDefault(item => item.Id == id)!);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(filter);
    }
    /// <summary>
    /// Returns a collection of entities that meet the condition
    /// </summary>
    /// <param name="filter">some condition</param>
    /// <returns>A collection of entities that meet the condition</returns>
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers
               select item;
    }

    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item">A reference to an updated existing object of type Engineer.</param>
    /// <exception cref="NotImplementedException">If there is no object with the received ID number - an exception will be thrown</exception>



    public void Update(Engineer item)
    {
        if (DataSource.Engineers.Find(d => d.Id == item.Id) == null)
        {
            throw new NotImplementedException($"Engineer with ID={item.Id} does Not exist");
        }
        Delete(DataSource.Engineers.FirstOrDefault(engineers => engineers.Id == item.Id).Id);
        DataSource.Engineers.Add(item);
        return;
    }
}

