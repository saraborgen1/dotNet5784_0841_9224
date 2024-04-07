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

    /// <summary>
    /// a function to add Month
    /// </summary>
    public void AddMonth();
    /// <summary>
    /// a function to add Day
    /// </summary>
    public void AddDay();
    /// <summary>
    /// a function to add Week
    /// </summary>
    public void AddWeek();
    /// <summary>
    /// Minimum Days from the project
    /// </summary>
    /// <returns></returns>
    public TimeSpan MinimumDays();
    /// <summary>
    /// set project dasys
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endStart"></param>
    public void SetProjectDates(DateTime startDate,DateTime endStart);

}
