namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int newId = DataSource.Config.NextDependencyId;
        Dependency newDependency = item with { Id = newId };
        DataSource.Dependencys.Add(newDependency);
        return newId;
    }

    public void Delete(int id)
    {
        if (DataSource.Dependencys.Find(d => d.Id == id) == null)
        {
            throw new NotImplementedException($"Dependency with ID={id} does Not exist");
        }
        DataSource.Dependencys.Remove(DataSource.Dependencys.Find(d => d.Id == id)!);

    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencys.FirstOrDefault(d => d.Id == id);
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencys);
    }

    public void Update(Dependency item)
    {
        if (DataSource.Dependencys.Find(d => d == item) == null) 
        {
            throw new NotImplementedException($"Dependency with ID={item.Id} does Not exist");
        }
        Dependency dependency = DataSource.Dependencys.Find(d => d == item)!;
        Dependency temp = dependency;
        DataSource.Dependencys.Remove(temp);
        DataSource.Dependencys.Add(item);
        return;
    }
}
