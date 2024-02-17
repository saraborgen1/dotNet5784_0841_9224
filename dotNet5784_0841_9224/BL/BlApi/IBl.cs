namespace BlApi;
public interface IBl
{
    /// <summary>
    /// property of Task interface
    /// </summary>
    public ITask Task { get; }

    /// <summary>
    /// property of Engineer interface
    /// </summary>
    public IEngineer Engineer { get; }

    /// <summary>
    /// property of State interface
    /// </summary>
    public IState State { get; }

    /// <summary>
    /// function that initializes database
    /// </summary>
    public void InitializeDB();

    /// <summary>
    /// function that clears database
    /// </summary>
    public void ResetDB();
}
