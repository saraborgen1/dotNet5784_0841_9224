namespace BlApi;

public interface IEngineer
{
    /// <summary>
    /// Returns all entitys from the list that meets the condition
    /// </summary>
    /// <param name="filter">condition</param>
    /// <returns>Returns all entitys from the list that meets the condition</returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null);

    /// <summary>
    /// Returns an entity from the list that id matches param
    /// </summary>
    /// <param name="id">condition</param>
    /// <returns>an entity from the list that meets the condition</returns>
    public BO.Engineer? Read(int id);

    /// <summary>
    /// Adding a new object to  database
    /// </summary>
    /// <param name="item"> An object of type Bo</param>
    public void Create(BO.Engineer item);

    /// <summary>
    /// Deletion of an existing object with a certain ID, from the list of entity type objects.
    /// </summary>
    /// <param name="id">condition</param>
    public void Delete(int id);

    /// <summary>
    /// Update of an existing object
    /// </summary>
    /// <param name="item">An object of type Bo</param>
    public void Update(BO.Engineer item);

    /// <summary>
    /// returns all deleted engineer
    /// </summary>
    /// <returns>all deleted engineer</returns>
    public IEnumerable<BO.Engineer> ReadAllDelete();

    /// <summary>
    /// deletes all objects of the entity type
    /// </summary>
    public void DeleteAll();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetPassword(int id);
}
