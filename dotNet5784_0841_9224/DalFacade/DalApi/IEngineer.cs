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

}

