using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

internal class StateImplementation : IDal
{
    public ITask Task => throw new NotImplementedException();

    public IDependency Dependency => throw new NotImplementedException();

    public IEngineer Engineer => throw new NotImplementedException();

    public DateTime? StartProject { get => DataSource.Config.startProject; set => DataSource.Config.startProject=value ; }
    public DateTime? EndProject { get => DataSource.Config.endProject; set => DataSource.Config.endProject=value; }

    public ProjectStatus StatusProject()
    {
        if (StartProject == null)
            return ProjectStatus.Creation;
        if (Task.Read(p => p.StartDate == null) != null)
            return ProjectStatus.Scheduling;
        return ProjectStatus.Start;
    }
}
