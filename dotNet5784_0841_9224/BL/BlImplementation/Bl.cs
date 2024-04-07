namespace BlImplementation;
using BlApi;

internal class Bl : IBl
{
    //factory
    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// access veriable
    /// </summary>
    public ITask Task => new TaskImplementation();
    /// <summary>
    /// access veriable
    /// </summary>
    public IEngineer Engineer => new EngineerImplementation();
    /// <summary>
    /// access veriable
    /// </summary>
    public IState State => new StateImplementation();
    /// <summary>
    /// ititialize
    /// </summary>
    public void InitializeDB() => DalTest.Initialization.Do();
    /// <summary>
    /// reset
    /// </summary>
    public void ResetDB() => DalTest.Initialization.Reset();
    /// <summary>
    /// admin password prop
    /// </summary>
    public string AdminPassword
    {
        get
        {
            return _dal.AdminPassword;
        }
    }
    /// <summary>
    /// admin id prop
    /// </summary>
    public int AdminUserId
    {
        get
        {
            return _dal.AdminUserId;
        }
    }

}
