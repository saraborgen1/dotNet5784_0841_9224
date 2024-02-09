using System.Diagnostics;

namespace Dal;
internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";
    private const string _entityName = nameof(Engineer);

    /// <summary>
    /// Adding a new object  of type Engineer to a database, (to the list of objects of type Engineer).
    /// </summary>
    /// <param name="item">A reference to an existing object of the Engineer type. The object was created in an upper layer and its fields are already filled with normal values.</param>
    /// <returns>Returns the id of the newly added object</returns>
    /// <exception cref="DalAlreadyExistsException">In case of an attempt to add an object that already exists - an exception will be thrown</exception>
    public int Create(Engineer item)
    {
        var listEngineer = LoadListFromXMLSerializer<Engineer>(s_engineers_xml);


        if (listEngineer.Exists(lec => lec?.Id == item.Id && lec.Active))
            throw new DalAlreadyExistsException(item.Id,_entityName);

        listEngineer.Add(item);
        SaveListToXMLSerializer(listEngineer, s_engineers_xml);

        return item.Id;
    }

    /// <summary>
    /// Deletion of an existing object with a certain ID, from the list of Engineer type objects.
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <exception cref="DalDoesNotExistsException">If there is no object with the received ID number - an appropriate exception will be thrown</exception>
    public void Delete(int id)
    {
        var listEngineer = LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        if ((listEngineer.FirstOrDefault(p => p.Id == id))==null)
            throw new DalDoesNotExistsException(id, _entityName);

        var newListEngineer = listEngineer.Select(item =>
        {
            if (item.Id == id)
            {
                item.Active = false;
                return item;
            }
            else
                return item;
        }) .ToList();
        SaveListToXMLSerializer(newListEngineer, s_engineers_xml);
    }

    /// <summary>
    /// Returns an entity from the list that meets the condition
    /// </summary>
    /// <param name="filter">condition</param>
    /// <returns>Returns an entity from the list that meets the condition</returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        var listEngineer = LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        return (ReadAll(item=>item.Active==true)).FirstOrDefault(filter);
    }
    
    /// <summary>
    /// Returns an entity from the list that meets the condition
    /// </summary>
    /// <param name="filter">condition</param>
    /// <returns>Returns an entity from the list that meets the condition</returns>
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool> filter = null!)
    {
        var listEngineer = LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        if (filter != null)
            return (from item in listEngineer
                   where (item.Active) && filter(item)
                   select item).ToList();

            return (from item in listEngineer
                    where (item.Active) 
                    select item).ToList();
    }

    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item">A reference to an updated existing object of type Engineer</param>
    public void Update(Engineer item)
    {
        var listEngineer = LoadListFromXMLSerializer<DO.Task>(s_engineers_xml);

        if (listEngineer.RemoveAll(p => p.Id == item.Id) == 0)
            throw new DalDoesNotExistsException(item.Id, _entityName);
        Create(item);
    }
    /// <summary>
    /// print all deleted engineer
    /// </summary>
    /// <returns> deleted engineer collection</returns>
    public IEnumerable<Engineer> ReadAllDelete()
    {
        var listEngineer = LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        return from item in listEngineer
               where !item.Active
               select item;
    }
    public void DeleteAll()
    {
        List<Engineer> newListEngineer = new List<Engineer>(); // Create a new empty list
        SaveListToXMLSerializer(newListEngineer, s_engineers_xml); // Save the empty list to XML
    }
}
