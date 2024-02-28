using System.Diagnostics;

namespace Dal;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    /// <summary>
    /// The realization of the entitie
    /// </summary>
    public ITask Task => new TaskImplementation();
    /// <summary>
    /// The realization of the entitie
    /// </summary>

    public IDependency Dependency => new DependencyImplementation();
    /// <summary>
    /// The realization of the entitie
    /// </summary>

    public IEngineer Engineer => new EngineerImplementation();

    /// <summary>
    /// An propotie that represents the project start date
    /// </summary>
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

    /// <summary>
    /// An propotie that represents the project end date
    /// </summary
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
    /// <summary>
    /// A function that returns the project status
    /// </summary>
    /// <returns>project status</returns>
    public ProjectStatus StatusProject()
    {
        if (StartProject == null)
            return ProjectStatus.Creation;
        if (Task.Read(p => p.StartDate == null) != null)
            return ProjectStatus.Scheduling;
        return ProjectStatus.Start;
    }
    /// <summary>
    /// propotie of  Current time project
    /// </summary>

    public DateTime CurrentDate
    {
        get
        {
            XElement root = XElement.Load(@"..\xml\data-config.xml");

            if (DateTime.TryParse((string?)root.Element("CurrentDate"), out var result))
                return result;
            CurrentDate=DateTime.Now;
            return CurrentDate;
         //   DateTime.TryParse((string?)root.Element("CurrentDate"), out var result) ? (DateTime?)result : null;
          //  DateTime.TryParse((string?)root.Element("CurrentDate"), out var result);

          //  return result;

        }
        set
        {
            XElement root = XElement.Load(@"..\xml\data-config.xml");
            root.Element("CurrentDate")?.SetValue(value.ToString("yyyy-MM-dd"));
            root.Save(@"..\xml\data-config.xml");
        }
    }
}
