namespace DalApi;
using DO;
/// <summary>
/// An interface that inherits from an interface
/// </summary>
public interface IEngineer : ICrud<Engineer>
{ 
    public IEnumerable<Engineer> ReadAllDelete();
}

