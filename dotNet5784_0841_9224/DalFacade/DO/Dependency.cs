namespace DO;

public record Dependency
(
    int Id,
    int? DependentTask=null,
    int? DependentOnTask=null
)
{
    public Dependency() : this(0) { } //empty ctor for stage 3
    public Dependency(int _Id, int _DependentTask, int _DependentOnTask)
    {
        Id = _Id;
        DependentTask = _DependentTask;
        DependentOnTask = _DependentOnTask;
    }
}
