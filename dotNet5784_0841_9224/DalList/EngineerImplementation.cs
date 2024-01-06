namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    /// <summary>
    /// Adding a new object of type Engineer to a database, (to the list of objects of type Engineer).
    /// </summary>
    /// <param name="item">A reference to an existing object of the Engineer type. The object was created in an upper layer and its fields are already filled with normal values.</param>
    /// <returns>The method will return the running number of the newly created object in the list.</returns>
    /// <exception cref="NotImplementedException">In case of an attempt to add an object that already exists - an exception will be thrown</exception>
    public int Create(Engineer item)
    {
        if (DataSource.Dependencys.Find(d => d.Id ==item.Id) != null)
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
        if (DataSource.Engineers.Find(d => d.Id == id) == null)
        {
            throw new NotImplementedException($"Engineer with ID={id} does Not exist");
        }
        DataSource.Engineers.Remove(DataSource.Engineers.Find(d => d.Id == id)!);
    }
    /// <summary>
    /// Returning a reference to a single object of type Engineer with a certain ID
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <returns>If the object exists, the method will return a reference to the existing object. Otherwise, the method will return null.</returns>
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(d => d.Id == id);
    }
    /// <summary>
    /// Returning a copy of the list of references to all objects of type Engineer
    /// </summary>
    /// <returns>The method returns a new list that is a copy of the existing list of all objects of type Engineer.</returns>
    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
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
        Engineer engineer;
        Delete((DataSource.Engineers.Find(d => d.Id == item.Id)).Id);
        //DataSource.Engineers.Remove(engineer);
        DataSource.Engineers.Add(item);
        return;
    }
}

