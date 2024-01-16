namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using static XMLTools;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencys_xml = "dependencys";

    public int Create(Dependency item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        var dependency = LoadListFromXMLElement(s_dependencys_xml).Descendants()
 .FirstOrDefault(e => int.Parse(e.Element(nameof(Dependency.Id))!.Value) == id);
    }

    public Dependency? Read(int id)
   => Read(dependency => dependency.Id == id);

    public Dependency? Read(Func<Dependency, bool> filter)
    => xelementToItem<Dependency>(LoadListFromXMLElement(s_dependencys_xml)); 

    //private Dependency getDependencyFromXElement(XElement element)
    //    => new Dependency(int.Parse(element.Element("Id")!.Value), int.Parse(element.Element("DependentTask")!.Value), int.Parse(element.Element("DependentOnTask")!.Value));

    //public IEnumerable<Dependency> ReadAll(Func<Dependency, bool> filter = null)
    //{
    //    var elements = XMLTools.LoadListFromXMLElement(s_dependencys_xml).Elements();

    //    return from element in elements
    //           let dependency = getDependencyFromXElement(element)
    //           where filter is null ? true : filter(dependency)
    //           select dependency;
    //}

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool> filter = null)
        => xelementToItems<Dependency>(LoadListFromXMLElement(s_dependencys_xml));

    public void Update(Dependency item)
    {
        throw new NotImplementedException();
    }
}
