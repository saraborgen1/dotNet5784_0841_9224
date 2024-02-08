namespace BlApi;

public interface IState 
{
    public void UpdateState();
    public DateTime StartProject { init; get; }
    public DateTime EndProject { init; get; }
    public BO.Enums.ProjectStatus StatusProject { set; get; }


}
