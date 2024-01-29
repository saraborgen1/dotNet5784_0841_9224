using System.Xml.Linq;

namespace DO;
/// <summary>
/// If an entity with this id is not found
/// </summary>
[Serializable]
public class DalDoesNotExistsException : Exception
{
    public DalDoesNotExistsException(int id ,string name) : base($"{name} with ID={id} does Not exist") { }
}
/// <summary>
/// The id number already exists
/// </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(int id, string name) : base($"{name} with ID={id} already exist") { }
}
/// <summary>
/// File exception
/// </summary>
[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}

