namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        foreach (Engineer temp in DataSource.Engineers)
        {
            if (item.Id == temp.Id)
            {
                throw new NotImplementedException($"Student with ID={item.Id} already exist");
            }
        }
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Engineer? Read(int id)
    {
        foreach (Engineer item in DataSource.Engineers)
        {
            if (item.Id == id) return item;
        }
        return null;
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        foreach (Engineer engineer in DataSource.Engineers)
        {
            if (engineer.Id == item.Id)
            {
                Engineer temp = engineer;
                DataSource.Engineers.Remove(temp);
                DataSource.Engineers.Add(item);
                return;
            }
        }
        throw new NotImplementedException($"An object of type Task with such an ID={item.Id} does not exist");
       
    }
}
