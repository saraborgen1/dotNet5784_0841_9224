using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
sealed public class DalList : IDal
{
    /// <summary>
    /// Interface property implement
    /// </summary>
    public ITask Task => new TaskImplementation();
    /// <summary>
    /// Interface property implement
    /// </summary>
    public IDependency Dependency => new DependencyImplementation();
    /// <summary>
    /// Interface property implement
    /// </summary>
    public IEngineer Engineer => new EngineerImplementation();
}

