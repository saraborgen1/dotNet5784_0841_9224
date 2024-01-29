namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;


internal class DependencyImplementation : IDependency
{
    private const string _entityName = nameof(Dependency);
    /// <summary>
    /// Adding a new object of type Dependency to a database, (to the list of objects of type Dependency).
    /// </summary>
    /// <param name="item">A reference to an existing object of type Dependency.</param>
    /// <returns>The method will return the running number of the newly created object in the list</returns>
    public int Create(Dependency item)
    {
        int newId = DataSource.Config.NextDependencyId;
        Dependency newDependency = item with { Id = newId };
        DataSource.Dependencys.Add(newDependency);

        return newId;
    }

    /// <summary>
    /// Deletion of an existing object with a certain ID, from the list of objects of type Dependency
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <exception cref="NotImplementedException">An attempt to delete an object that does not exist</exception>
    public void Delete(int id)
    {
        if (DataSource.Dependencys.FirstOrDefault(item => item.Id == id) == null)
            throw new DalDoesNotExistsException(id, _entityName);

        DataSource.Dependencys.Remove(DataSource.Dependencys.FirstOrDefault(item => item.Id == id)!);
    }
    /// <summary>
    /// Returns an entity from the list that meets the condition
    /// </summary>
    /// <param name="filter">condition</param>
    /// <returns>Returns an entity from the list that meets the condition</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencys.FirstOrDefault(filter);
    }
    /// <summary>
    /// Returns a collection of entities that meet the condition
    /// </summary>
    /// <param name="filter">some condition</param>
    /// <returns>A collection of entities that meet the condition</returns>
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null) //stage 2
    {
        if (filter != null)
            return (from item in DataSource.Dependencys
                   where filter(item)
                   select item).ToList();

        return (from item in DataSource.Dependencys
               select item).ToList();
    }

    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with 
    /// a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item">A reference to an updated existing object of type Dependency.</param>
    /// <exception cref="NotImplementedException">In case there is no object with the received id</exception>
    public void Update(Dependency item)
    {
        if (DataSource.Dependencys.Find(d => d.Id == item.Id) == null) 
            throw new DalDoesNotExistsException(item.Id, _entityName);

        Delete(item.Id);
        DataSource.Dependencys.Add(item);
    }
}
