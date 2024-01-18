namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Linq;
using static XMLTools;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencys_xml = "dependencys";

    public int Create(Dependency item)
    {
        int newId = Config.NextDependencyId;
        Dependency newDependency = item with { Id = newId };
        XElement dependencysRootElem = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        XElement dependency = itemToXelement(newDependency, "Dependency");
        dependencysRootElem.Add( dependency);
        XMLTools.SaveListToXMLElement(dependencysRootElem, s_dependencys_xml);
        return newId;
    }

    public void Delete(int id)
    {
        XElement dependencysRootElem = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        if (dependencysRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == id) == null)
            throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");
        dependencysRootElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == id)!.Remove();
        XMLTools.SaveListToXMLElement(dependencysRootElem, s_dependencys_xml);
    }

    public Dependency? Read(int id)
   => Read(dependency => dependency.Id == id);

    public Dependency? Read(Func<Dependency, bool> filter)
     => xelementToItems<Dependency>(LoadListFromXMLElement(s_dependencys_xml)).FirstOrDefault(filter);

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
    {
       var listDependency= xelementToItems<Dependency>(LoadListFromXMLElement(s_dependencys_xml)).ToList();
        return from element in listDependency
               where filter is null ? true : filter(element)
               select element;

    }
    //public IEnumerable<Dependency> ReadAll(Func<Dependency, bool> filter = null)
    //    => xelementToItems<Dependency>(LoadListFromXMLElement(s_dependencys_xml));


    public void Update(Dependency item)
    {
        var listDependency = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        Delete(item.Id);
        XElement dependency = itemToXelement(item, "Dependency");
        listDependency.Add(dependency);
        XMLTools.SaveListToXMLElement(listDependency, s_dependencys_xml);
    }
}
