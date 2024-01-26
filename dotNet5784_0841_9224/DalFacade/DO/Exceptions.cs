namespace DO;
/// <summary>
/// If an entity with this id is not found
/// </summary>
[Serializable]
public class DalDoesNotExistsException : Exception
{
    public DalDoesNotExistsException(string? message) : base(message) { }
}
/// <summary>
/// The id number already exists
/// </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}
/// <summary>
/// File exception
/// </summary>
[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}

