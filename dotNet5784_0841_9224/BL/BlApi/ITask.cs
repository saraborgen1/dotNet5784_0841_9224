namespace BlApi;

public interface ITask
{
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool> filter = null!);
    public BO.Task? Read(Func<BO.Task, bool> filter);
    public void Create(BO.Task item);
    public void Update(BO.Task item);
    public void Delete(int id);
    public void UpdateDate(int id, DateTime date);

}
