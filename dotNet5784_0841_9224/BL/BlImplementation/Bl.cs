namespace BlImplementation;
using BlApi;
internal class Bl : IBl
{
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();
    public DateTime ProjectCreate
    {

    }
    public DateTime ProjectEnd 
    public BO.Enums.ProjectStatus getState
}
