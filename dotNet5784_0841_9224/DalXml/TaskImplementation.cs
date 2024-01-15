namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

internal class TaskImplementation:ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task newTask = item with { Id = newId };
        DataSource.Tasks.Add(newTask);
        return newId;

    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task? Read(Func<Task, bool> filter)
    {
        
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        var taskList=LoadListFromXMLSerializer()
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item;
    }

    public void Update(Task item)
    {
        throw new NotImplementedException();
    }
}
