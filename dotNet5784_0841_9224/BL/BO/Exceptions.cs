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
    public BlCannotBeDeletedException(int id, string message) : base($"{message} with ID={id} cannot be deleted because" ) { }

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
    public BlDoesNotExistException(Exception innerException)
            : base(innerException.Message) { }
}
public class BlNoDateException : Exception
{
    public BlNoDateException(string name): base(name) { }
}
public class BlDateClashException : Exception
{
    public BlDateClashException(string name) : base(name) { }
}
public class BlCannotBeDeletedWrongStateException: Exception
{
   public BlCannotBeDeletedWrongStateException(string problem):base(problem) { }
}
public class BlCannotAddWrongStateException : Exception
{
    public BlCannotAddWrongStateException(string problem) : base(problem) { }
}


    public class BlCannotUpdateWrongStateException : Exception
{
    public BlCannotUpdateWrongStateException(string problem) : base(problem) { }
}