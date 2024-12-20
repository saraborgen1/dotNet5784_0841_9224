﻿namespace Dal;


internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";
    private const string _entityName = nameof(DO.Task);

    /// <summary>
    /// Adding a new object of type Task to a database, (to the list of objects of type Task).
    /// </summary>
    /// <param name="item">A reference to an existing object of type Task</param>
    /// <returns>The method will return the running number of the newly created object in the list</returns>
    public int Create(DO.Task item)
    {
        int newId = Config.NextTaskId;
        DO.Task newTask = item with { Id = newId };
        var taskList = LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        taskList.Add(newTask);
        SaveListToXMLSerializer(taskList, s_tasks_xml);

        return newId;
    }
    /// <summary>
    /// Deletion of an existing object with a certain ID, from the list of objects of type Task
    /// </summary>
    /// <param name="id">ID number of an object</param>
    /// <exception cref="DalDoesNotExistsException">An attempt to delete an object that does not exist</exception>
    public void Delete(int id)
    {
        var taskList = LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        if (taskList.RemoveAll(item => item.Id == id) == 0)
            throw new DalDoesNotExistsException(id, _entityName);

        SaveListToXMLSerializer(taskList, s_tasks_xml);
    }

    /// <summary>
    /// Returns an entity from the list that meets the condition
    /// </summary>
    /// <param name="filter">condition</param>
    /// <returns>Returns an entity from the list that meets the condition</returns>
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        var taskList = LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        return taskList.FirstOrDefault(filter);
    }

    /// <summary>
    /// Returns a collection of entities that meet the condition
    /// </summary>
    /// <param name="filter">some condition</param>
    /// <returns>A collection of entities that meet the condition</returns>
    public IEnumerable<DO.Task> ReadAll(Func<DO.Task, bool> filter = null!)
    {
        var taskList = LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        if (filter != null)
            return (from item in taskList
                    where filter(item)
                    select item).ToList();

        return taskList.ToList();
    }

    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with 
    /// a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item">A reference to an updated existing object of type Task</param>
    public void Update(DO.Task item)
    {
        Delete(item.Id);

        var taskList = LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        taskList.Add(item);

        SaveListToXMLSerializer(taskList, s_tasks_xml);
    }
    /// <summary>
    /// A function that deletes all objects of the entity type
    /// </summary>
    public void DeleteAll()
    {
        List<DO.Task> newListTask = new List<DO.Task>(); // Create a new empty list
        SaveListToXMLSerializer(newListTask, s_tasks_xml); // Save the empty list to XML

        //Initialization of the running number of the id
        XElement root = LoadListFromXMLElement("data-config");
        root.Element("NextTaskId")?.SetValue(1);
        SaveListToXMLElement(root, "data-config");
    }

}
