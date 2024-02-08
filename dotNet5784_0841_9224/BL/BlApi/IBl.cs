namespace BlApi;
public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public DateTime ProjectCreate { init; get; }
    public DateTime ProjectEnd { init; get; }
    public BO.Enums.ProjectStatus GetState();
    
}
