namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    private const string _entityName = nameof(Engineer);
    /// <summary>
    /// Adding a new object  of type Engineer to a database, (to the list of objects of type Engineer).
    /// </summary>
    /// <param name="item">A reference to an existing object of the Engineer type. The object was created in an upper layer and its fields are already filled with normal values.</param>
    /// <returns>The method will return the running number of the newly created object in the list.</returns>
    /// <exception cref="NotImplementedException">In case of an attempt to add an object that already exists - an exception will be thrown</exception>
    public int Create(Engineer item)
    {

        if (DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == item.Id) != null)
            throw new DalAlreadyExistsException(item.Id, _entityName);

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
        if (DataSource.Engineers.FirstOrDefault(item => item.Id == id && item.Active) == null)
            throw new DalDoesNotExistsException(id, _entityName);

        DataSource.Engineers.Remove(DataSource.Engineers.FirstOrDefault(item => item.Id == id)!);
    }
    /// <summary>
    /// Returns an entity from the list that meets the condition
    /// </summary>
    /// <param name="filter">condition</param>
    /// <returns>Returns an entity from the list that meets the condition</returns>
    public Engineer? Read(Func<Engineer, bool> filter)
        => (ReadAll(item => item.Active == true)).FirstOrDefault(filter);
    /// <summary>
    /// Returns an entity from the list that meets the condition
    /// </summary>
    /// <param name="filter">condition</param>
    /// <returns>Returns an entity from the list that meets the condition</returns>
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null) //stage 2
    {
        if (filter != null)
            return (from item in DataSource.Engineers
                   where filter(item) && item.Active
                   select item).ToList();

        return( from item in DataSource.Engineers
               where item.Active == true
               select item).ToList();
    }

    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item">A reference to an updated existing object of type Engineer.</param>
    /// <exception cref="NotImplementedException">If there is no object with the received ID number - an exception will be thrown</exception>
    public void Update(Engineer item)
    {
        if (DataSource.Engineers.RemoveAll(p => p.Id == item.Id) == 0)
            throw new DalDoesNotExistsException(item.Id, _entityName);
        DataSource.Engineers.Add(item);
    }
    /// <summary>
    /// print all deleted engineer
    /// </summary>
    /// <returns> deleted engineer collection</returns>
    public IEnumerable<Engineer> ReadAllDelete()
    {
        return (from item in DataSource.Engineers
               where !item.Active
               select item).ToList();
    }
    public void DeleteAll()
    {
        if (DataSource.Engineers.ReadAllDelete().FirstOrDefault(item => item.Id == id) == null)
            throw new DalDoesNotExistsException(id, _entityName);

        DataSource.Tasks.Remove(DataSource.Tasks.FirstOrDefault(item => item.Id == id)!);
    }
}

