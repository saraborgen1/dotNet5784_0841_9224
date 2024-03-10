using BO;

namespace BlApi;

public interface ITask
{
    /// <summary>
    /// Returns all entitys from the list that meets the condition
    /// </summary>
    /// <param name="filter">condition</param>
    /// <returns>Returns all entitys from the list that meets the condition</returns>
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool> ? filter = null);

    /// <summary>
    /// Returns an entity from the list that id matches param
    /// </summary>
    /// <param name="id">condition</param>
    /// <returns>an entity from the list that meets the condition</returns>
    public BO.Task Read(int id);

    /// <summary>
    /// Adding a new object to  database
    /// </summary>
    /// <param name="item"> An object of type Bo</param>
    public void Create(BO.Task item);

    /// <summary>
    /// Update of an existing object
    /// </summary>
    /// <param name="item">An object of type Bo</param>
    public void Update(BO.Task item);

    /// <summary>
    /// Deletion of an existing object with a certain ID, from the list of entity type objects.
    /// </summary>
    /// <param name="id">condition</param>
    public void Delete(int id);

    /// <summary>
    /// updates a date of a task
    /// </summary>
    /// <param name="id">id of entity to update</param>
    /// <param name="date"> new date</param>
    public void UpdateDate(int id, DateTime date);

    public void AutoScheduling();
    public TaskInList GetTaskInList(int id);
    public List<TaskInList> GetAllTaskInList(int id);


}
