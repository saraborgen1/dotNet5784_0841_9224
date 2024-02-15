namespace BlApi;
public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public IState State { get; }
    public void InitializeDB();
    public void ResetDB();
}
