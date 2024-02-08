namespace BlImplementation;
using BlApi;
using BO;

internal class Bl : IBl
{
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

}
