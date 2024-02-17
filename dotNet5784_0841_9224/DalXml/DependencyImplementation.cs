namespace Dal;

using DalApi;
using DO;
using static XMLTools;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencys_xml = "dependencys";

    private const string _entityName = nameof(Dependency);
    /// <summary>
    /// Adding a new object  of type Dependency to a database, (to the list of objects of type Dependency).
    /// </summary>
    /// <param name="item">A reference to an existing object of the Engineer type. The object was created in an upper layer and its fields are already filled with normal values.</param>
    /// <returns>The id of the newly added object</returns>
    public int Create(Dependency item)
    {
        int newId = Config.NextDependencyId;
        Dependency newDependency = item with { Id = newId };

        XElement dependencysRootElem = LoadListFromXMLElement(s_dependencys_xml);

        XElement dependency = itemToXelement(newDependency, _entityName);
        dependencysRootElem.Add( dependency);
        SaveListToXMLElement(dependencysRootElem, s_dependencys_xml);

        return newId;
    }

    /// <summary>
    /// Deletion of an existing object with a certain ID, from the list of objects of type dependency
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <exception cref="DalDoesNotExistsException">An attempt to delete an object that does not exist</exception>
    public void Delete(int id)
    {
        XElement dependencysRootElem =LoadListFromXMLElement(s_dependencys_xml);

        if (dependencysRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == id) == null)
            throw new DalDoesNotExistsException(id, _entityName);

        dependencysRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == id)!.Remove();
      
        SaveListToXMLElement(dependencysRootElem, s_dependencys_xml);
    }

    /// <summary>
    /// Since it is very useful, it always receives an id and returns a suitable object
    /// </summary>
    /// <param name="id">id by which we will return an object</param>
    /// <returns>suitable object</returns>
    public Dependency? Read(int id)
   => Read(dependency => dependency.Id == id);

    /// <summary>
    /// Accepts a condition and returns an object that meets the condition
    /// </summary>
    /// <param name="filter">condition by which we will return an object</param>
    /// <returns>suitable object</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
     => xelementToItems<Dependency>(LoadListFromXMLElement(s_dependencys_xml)).FirstOrDefault(filter);

    /// <summary>
    /// Accepts a condition and returns a collection of objectt that meet the condition
    /// </summary>
    /// <param name="filter">condition by which we will select an object</param>
    /// <returns>collection of objectt that meet the condition</returns>
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool> filter = null!)
    {
       var listDependency= xelementToItems<Dependency>(LoadListFromXMLElement(s_dependencys_xml)).ToList();
        return (from element in listDependency
               where filter is null ? true : filter(element)
               select element).ToList();
    }

    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with 
    /// a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item">A reference to an updated existing object of type Dependency</param>
    public void Update(Dependency item)
    {
        var listDependency = LoadListFromXMLElement(s_dependencys_xml);
        Delete(item.Id);
        XElement dependency = itemToXelement(item, _entityName);
        listDependency.Add(dependency);
        SaveListToXMLElement(listDependency, s_dependencys_xml);
    }
    /// <summary>
    /// A function that deletes all objects of the entity type
    /// </summary>
    public void DeleteAll()
    {
        List<Dependency> newListDependency = new List<Dependency>(); // Create a new empty list
        SaveListToXMLSerializer(newListDependency, s_dependencys_xml); // Save the empty list to XML

        //Initialization of the running number of the id
        XElement root = LoadListFromXMLElement("data-config");
        root.Element("NextDependencyId")?.SetValue(1);
        SaveListToXMLElement(root, "data-config");
    }
}
