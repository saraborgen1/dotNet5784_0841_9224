namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(Task item)
    {
        throw new NotImplementedException();
    }



    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task? Read(Func<Task, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool> filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Task item)
    {
        throw new NotImplementedException();
    }

    public void UpdateDate(int id, DateTime date)
    {
        throw new NotImplementedException();
    }
}
