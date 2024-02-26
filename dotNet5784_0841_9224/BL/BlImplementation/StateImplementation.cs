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
            return _dal.StartProject;
        }
        set
        {
            _dal.StartProject = value;
        }

    }


    public DateTime? EndProject
    {
        get
        {
            return _dal.EndProject;
        }
        set
        {
            _dal.EndProject = value;
        }
    }

    public BO.Enums.ProjectStatus StatusProject()
    {
        return (BO.Enums.ProjectStatus)_dal.StatusProject();
    }

    public DateTime? CurrentDate
    {
        get
        {
            return _dal.CurrentDate;
        }
        set
        {
            _dal.CurrentDate = value;
        }
    }
}
