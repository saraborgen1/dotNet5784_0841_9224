namespace BlImplementation;
using BlApi;

internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public IState State => new StateImplementation();
    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Reset();

    public int AdminPassword
    {
        get
        {
            return _dal.AdminPassword;
        }
    }

    public int AdminUserId
    {
        get
        {
            return _dal.AdminUserId;
        }
    }

}
