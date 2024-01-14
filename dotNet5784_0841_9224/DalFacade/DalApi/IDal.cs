using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi;

public interface IDal
{
    /// <summary>
    /// property of task interface
    /// </summary>
    ITask Task { get; }
    /// <summary>
    /// property of dependency interface
    /// </summary>
    IDependency Dependency { get; }
    /// <summary>
    /// property of engineer interface
    /// </summary>
    IEngineer Engineer { get; }
}
