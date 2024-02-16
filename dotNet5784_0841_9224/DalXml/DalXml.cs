﻿using System.Diagnostics;

namespace Dal;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }

    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public DateTime? StartProject
    {
        get
        {
            XElement root = LoadListFromXMLElement("data-config");
            return DateTime.TryParse((string?)root.Element("StartProject"), out var result) ? (DateTime?)result : null;
        }
        set
        {
            XElement root = LoadListFromXMLElement("data-config");
            root.Element("StartProject")?.SetValue(value == null ? "" : value.Value.ToString("yyyy-MM-dd"));
            SaveListToXMLElement(root, "data-config");
        }

    }


    public DateTime? EndProject
    {
        get
        {
            XElement root = XElement.Load(@"..\xml\data-config.xml");
            return DateTime.TryParse((string?)root.Element("EndProject"), out var result) ? (DateTime?)result : null;

        }
        set
        {
            XElement root = XElement.Load(@"..\xml\data-config.xml");
            root.Element("EndProject")?.SetValue(value == null ? "" : value.Value.ToString("yyyy-MM-dd"));
            root.Save(@"..\xml\data-config.xml");
        }
    }

    public ProjectStatus StatusProject()
    {
        if (StartProject == null)
            return ProjectStatus.Creation;
        if (Task.Read(p => p.StartDate == null) != null)
            return ProjectStatus.Scheduling;
        return ProjectStatus.Start;
    }
}
