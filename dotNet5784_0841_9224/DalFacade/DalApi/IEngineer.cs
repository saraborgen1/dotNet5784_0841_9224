namespace DalApi;
using DO;
/// <summary>
/// An interface that inherits from an interface
/// </summary>
public interface IEngineer : ICrud<Engineer>
{
    /// <summary>
    /// a function that returns all deleted Engineers
    /// </summary>
    /// <returns>deleted Engineers</returns>
    public IEnumerable<Engineer> ReadAllDelete();

    /// <summary>
    /// returns password of engineer with that id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>password</returns>
    public int? GetPassword(int id);
}

