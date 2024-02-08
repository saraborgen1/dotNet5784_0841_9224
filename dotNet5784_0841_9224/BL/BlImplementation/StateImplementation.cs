namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using System;

internal class StateImplementation : IState
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public DateTime StartProject { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
    public DateTime EndProject { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
    public BO.Enums.ProjectStatus StatusProject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


    public void UpdateState()
    {
        if (ProjectCreate == null)
            StatusProject= Enums.ProjectStatus.Creation;
        if (_dal.Task.Read(p => p.StartDate == null) != null)
            StatusProject= Enums.ProjectStatus.Scheduling;
        StatusProject= Enums.ProjectStatus.Start;
    }
}
