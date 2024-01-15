
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencys_xml = "dependencys";

    public int Create(Dependency item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {

        return filter is null ?
            XMLTools.LoadListFromXMLElement(s_dependencys_xml).Elements().Select(d => d)
        : XMLTools.LoadListFromXMLElement(s_dependencys_xml).Elements().Where(filter);
        //        : XMLTools.LoadListFromXMLElement(s_dependencys_xml).Elements().Select(d => d).Where(filter);
        //students = (from p in studentRoot.Elements()
        //            select new Student()
        //            {
        //                Id = Convert.ToInt32(p.Element("id").Value),
        //                FirstName = p.Element("name").Element("firstName").Value,
        //                LastName = p.Element("name").Element("lastName").Value
        //            }

        public void Update(Dependency item)
    {
        throw new NotImplementedException();
    }
}
