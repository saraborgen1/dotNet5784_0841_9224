
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task newTask = item with { Id = newId };
        DataSource.Tasks.Add(newTask);
        return newId;
    }

    public void Delete(int id)
    {
        //לבדוק אם מותר למחוק את היישות
        // throw new NotImplementedException("An object of type Task with such an ID does not exist");
        foreach (Task item in DataSource.Tasks)
        {
            if (item.Id == id)
            {
                DataSource.Tasks.Remove(item);
                return;
                //אם החלטנו לעשות לא פעיל לעבטד בהתאם
            }
        }
        throw new Exception($"Student with ID={id} does Not exist");
    }

    public Task? Read(int id)
    {
        foreach(Task item in DataSource.Tasks)
        {
            if(item.Id == id) return item;
        }
        return null;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        foreach(Task task in DataSource.Tasks)
        {
            if (task.Id == item.Id)
            {
                Task temp= task;
                DataSource.Tasks.Remove(temp);
                DataSource.Tasks.Add(item);
                return;
            }
        }
        throw new NotImplementedException("An object of type Task with such an ID does not exist");
    }
}
