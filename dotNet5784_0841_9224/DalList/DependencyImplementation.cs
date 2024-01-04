namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Threading.Tasks;


public class DependencyImplementation : IDependency
{
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
        if (DataSource.Dependencys.Find(d => d.Id == id) == null)
        {
            throw new NotImplementedException($"Dependency with ID={id} does Not exist");
        }
        DataSource.Dependencys.Remove(DataSource.Dependencys.Find(d => d.Id == id)!);

    }
    /// <summary>
    /// Returning a reference to a single object of type Dependency with a certain ID, if it exists in 
    ///a database (in a list of data of type Dependency), or null if the object does not exist.
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <returns>If there is an object in the database with the received ID number, the method
    /// will return a reference to the existing object.Otherwise, the method will return null.</returns>

    public Dependency? Read(int id)
    {
        return DataSource.Dependencys.FirstOrDefault(d => d.Id == id);
    }
    /// <summary>
    /// Return a copy of the list of references to all objects of type Dependency
    /// </summary>
    /// <returns>The method returns a new list that is a copy of the existing list of all objects of type Dependency.</returns>
    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencys);
    }
    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with 
    /// a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item">A reference to an updated existing object of type Dependency.</param>
    /// <exception cref="NotImplementedException">In case there is no object with the received id</exception>
    public void Update(Dependency item)
    {
        if (DataSource.Dependencys.Find(d => d == item) == null) 
        {
            throw new NotImplementedException($"Dependency with ID={item.Id} does Not exist");
        }
        Dependency dependency = DataSource.Dependencys.Find(d => d == item)!;
        Dependency temp = dependency;
        DataSource.Dependencys.Remove(temp);
        DataSource.Dependencys.Add(item);
        return;
    }
}
