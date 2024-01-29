namespace BO;

[Serializable]
public class BlAlreadyExistException : Exception
{
    public BlAlreadyExistException(int id, string message) : base($"{message} with ID={id} already exists") { }
    public BlAlreadyExistException(int id, string message, Exception innerException)
                : base(message, innerException) { }

    public BlAlreadyExistException(Exception innerException)
                : base(innerException) { }
}

[Serializable]
public class BlTheInputIsInvalidException : Exception
{
    public BlTheInputIsInvalidException(string name) : base($"The {name} is incorrect") { }
}


