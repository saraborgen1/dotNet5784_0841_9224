namespace BlApi;

public interface IState 
{
    public DateTime? StartProject { set; get; }
    public DateTime? EndProject { set; get; }
    public BO.Enums.ProjectStatus StatusProject();


}
