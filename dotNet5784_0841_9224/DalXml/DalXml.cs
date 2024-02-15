using System.Diagnostics;

namespace Dal;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }

    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public DateTime? StartProject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTime? EndProject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public ProjectStatus StatusProject()
    {
        throw new NotImplementedException();
    }
}
