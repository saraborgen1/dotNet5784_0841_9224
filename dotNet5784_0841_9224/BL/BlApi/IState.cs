namespace BlApi;

public interface IState 
{
    /// <summary>
    /// propotie of start date project
    /// </summary>
    public DateTime? StartProject { set; get; }

    /// <summary>
    /// propotie of start date project
    /// </summary>
    public DateTime? EndProject { set; get; }

    /// <summary>
    /// function that returns projest status
    /// </summary>
    /// <returns>projest status</returns>
    public BO.Enums.ProjectStatus StatusProject();

    /// <summary>
    /// propotie of  Current time project
    /// </summary>

    public DateTime CurrentDate { set; get; }

    /// <summary>
    /// a function to add year
    /// </summary>
    public void AddYear();



}
