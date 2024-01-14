namespace DO;
/// <summary>
/// If an entity with this id is not found
/// </summary>
[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}
/// <summary>
/// The id number already exists
/// </summary>
[Serializable]
public class DalAlreadyExistException : Exception
{
    public DalAlreadyExistException(string? message) : base(message) { }
}

