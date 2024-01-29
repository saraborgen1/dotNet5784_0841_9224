namespace BO;

[Serializable]
public class BlAlreadyExistException : Exception
{
    public BlAlreadyExistException(int id, string message, Exception innerException) : base($"{message} with ID={id} already exists") { }

    public BlAlreadyExistException(Exception innerException)
                : base(innerException.Message) { }
}

[Serializable]
public class BlCannotBeDeletedException : Exception
{
    public BlCannotBeDeletedException(int id, string message, Exception innerException) : base($"{message} with ID={id} cannot be deleted because"+ innerException) { }
   
    public BlCannotBeDeletedException(Exception innerException)
                : base(innerException.Message) { }
}

[Serializable]
public class BlTheInputIsInvalidException : Exception
{
    public BlTheInputIsInvalidException(string name) : base($"The {name} is incorrect") { }
}

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(int id, string name) : base($"{name} with ID={id} does Not exist") { }
}



