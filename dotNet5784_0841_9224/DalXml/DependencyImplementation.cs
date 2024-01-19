namespace Dal;

using static XMLTools;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencys_xml = "dependencys";

    private const string _entityName = nameof(Dependency);
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

    public void Delete(int id)
    {
        XElement dependencysRootElem =LoadListFromXMLElement(s_dependencys_xml);
        if (dependencysRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == id) == null)
            throw new DalDoesNotExistException($"_entityName with ID={id} does Not exist");
        dependencysRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == id)!.Remove();
        SaveListToXMLElement(dependencysRootElem, s_dependencys_xml);
    }

    public Dependency? Read(int id)
   => Read(dependency => dependency.Id == id);

    public Dependency? Read(Func<Dependency, bool> filter)
     => xelementToItems<Dependency>(LoadListFromXMLElement(s_dependencys_xml)).FirstOrDefault(filter);

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool> filter = null!)
    {
       var listDependency= xelementToItems<Dependency>(LoadListFromXMLElement(s_dependencys_xml)).ToList();
        return from element in listDependency
               where filter is null ? true : filter(element)
               select element;
    }

    public void Update(Dependency item)
    {
        var listDependency = LoadListFromXMLElement(s_dependencys_xml);
        Delete(item.Id);
        XElement dependency = itemToXelement(item, _entityName);
        listDependency.Add(dependency);
        SaveListToXMLElement(listDependency, s_dependencys_xml);
    }
}
