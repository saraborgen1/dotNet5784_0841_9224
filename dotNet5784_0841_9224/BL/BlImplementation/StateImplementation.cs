namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Xml.Linq;
using static Dal.XMLTools;
internal class StateImplementation : IState
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
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
            root.Element("StartProject")?.SetValue(value!);
            SaveListToXMLElement(root, "data-config");
        }

    }


    public DateTime? EndProject
    {
        get
        {
            XElement root = XElement.Load(@"..\xml\data_config.xml");
            return DateTime.TryParse((string?)root.Element("EndProject"), out var result) ? (DateTime?)result : null;

        }
        set
        {
            XElement root = XElement.Load(@"..\xml\data_config.xml"); ;
            root.Element("EndProject")?.SetValue(value?.ToString());
            root.Save(@"..\xml\data_config.xml");
        }
    }
    public BO.Enums.ProjectStatus StatusProject { get; set;}


    public void UpdateState()
    {
        if (StartProject == null)
            StatusProject = Enums.ProjectStatus.Creation;
        if (_dal.Task.Read(p => p.StartDate == null) != null)
            StatusProject = Enums.ProjectStatus.Scheduling;
        StatusProject = Enums.ProjectStatus.Start;
    }
}
