namespace BlApi;
public interface IBl
{
    public ITask Task { get; }
    public IMilestone Milestone { get; }
    public IEngineer Engineer { get; }
}
