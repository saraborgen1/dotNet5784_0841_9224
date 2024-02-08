namespace BlImplementation;
using BlApi;
using BO;

internal class Bl : IBl
{
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    DateTime IBl.ProjectCreate { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
    DateTime IBl.ProjectEnd { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

    public Enums.ProjectStatus GetState()
    {
        throw new NotImplementedException();
    }
}
