namespace BO;

/// <summary>
/// Exception for if it already exists
/// </summary>
[Serializable]

public class BlAlreadyExistException : Exception
{
    public BlAlreadyExistException(int id, string message, Exception innerException) : base($"{message} with ID={id} already exists") { }

    public BlAlreadyExistException(Exception innerException)
                : base(innerException.Message) { }
    public BlAlreadyExistException(string message)
                : base(message) { }
}
/// <summary>
/// Exception for if it cant be deleted exists
/// </summary>

[Serializable]
public class BlCannotBeDeletedException : Exception
{
    public BlCannotBeDeletedException(int id, string message, Exception innerException) : base($"{message} with ID={id} cannot be deleted because" + innerException) { }
    public BlCannotBeDeletedException(int id, string message) : base($"{message} with ID={id} cannot be deleted because") { }

    public BlCannotBeDeletedException(Exception innerException)
                : base(innerException.Message) { }
}

/// <summary>
/// Exception for if the id is not valid
/// </summary>
[Serializable]
public class BlTheInputIsInvalidException : Exception
{
    public BlTheInputIsInvalidException(string name) : base($"The {name} is incorrect") { }
}

/// <summary>
/// Exception for if it doesnt exist
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(int id, string name) : base($"{name} with ID={id} does Not exist") { }
    public BlDoesNotExistException(Exception innerException)
            : base(innerException.Message) { }
}

/// <summary>
/// Exception for if a date is missing
/// </summary>
[Serializable]
public class BlNoDateException : Exception
{
    public BlNoDateException(string name) : base(name) { }
}

/// <summary>
/// Exception for if the dates clash 
/// </summary>
[Serializable]
public class BlDateClashException : Exception
{
    public BlDateClashException(string name) : base(name) { }
}

/// <summary>
/// Exception for if it cannot delete because its the wrong status
/// </summary>
[Serializable]
public class BlCannotBeDeletedWrongStateException : Exception
{
    public BlCannotBeDeletedWrongStateException(string problem) : base(problem) { }
}

/// <summary>
/// Exception for if it cannot add because its the wrong status
/// </summary>
[Serializable]
public class BlCannotAddWrongStateException : Exception
{
    public BlCannotAddWrongStateException(string problem) : base(problem) { }
}

/// <summary>
/// Exception for if it cannot update because its the wrong status
/// </summary>
[Serializable]
public class BlCannotUpdateWrongStateException : Exception
{
    public BlCannotUpdateWrongStateException(string problem) : base(problem) { }
}

/// <summary>
/// Cannot do auto scheduling exception
/// </summary>
[Serializable]
public class BlCannotDoAutoSchedulingException : Exception
{
    public BlCannotDoAutoSchedulingException(string problem) : base(problem) { }
}

/// <summary>
/// Cannot create a circular dependency
/// </summary>
[Serializable]
public class BlCirculDepException : Exception
{
    public BlCirculDepException(string problem) : base(problem) { }
}

