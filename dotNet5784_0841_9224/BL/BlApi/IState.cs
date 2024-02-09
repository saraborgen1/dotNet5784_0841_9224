namespace BlApi;

public interface IState 
{
    public void UpdateState();
    public DateTime? StartProject { set; get; }
    public DateTime? EndProject { set; get; }
    public BO.Enums.ProjectStatus StatusProject { set; get; }


}
