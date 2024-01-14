namespace DalTest;
using DalApi;
using DO;
using System.Security.Cryptography;

public static class Initialization
{
    /// <summary>
    /// access variable to  entitys
    /// </summary>
    private static IDal? s_dal;
    public static void Do(IDal dal)
    {
        s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        createTasks();
        createDependencys();
        createEngineers();
    }
    /// <summary>
    /// Generating random numbers while filling in the object values
    /// </summary>
    private static readonly Random s_rand = new();
    /// <summary>
    /// An operation that initializes Task type entities with data
    /// </summary>
   
    private static void createTasks()
    {
        string[] TaskDescriptions =
         {
         "nothing",
        "Planning the layout involves creating a blueprint for the building's design, considering spatial arrangements and functionality.",
        "Structural framing is the process of constructing the skeletal structure of the building, providing support and shape according to the plan.",
        "Electrical wiring encompasses the installation of electrical systems, ensuring proper connections and functionality throughout the building.",
        "Plumbing installation involves setting up the plumbing system, including pipes, fixtures, and ensuring efficient water distribution.",
        "HVAC Systems Setup focuses on installing heating, ventilation, and air conditioning systems to maintain a comfortable and controlled indoor environment.",
        "Drywall installation is the process of fixing gypsum boards to create interior walls, providing a smooth and finished surface.",
        "Flooring installation involves laying various types of flooring materials, enhancing the aesthetics and functionality of different spaces.",
        "Roofing installation includes constructing the roof structure and covering, protecting the building from weather elements.",
        "Exterior painting is the application of protective coatings and decorative finishes to the exterior surfaces, enhancing the building's appearance.",
        "Interior painting involves applying paint or finishes to interior walls, ceilings, and other surfaces, providing a desired aesthetic.",
        "Window installation focuses on placing windows, allowing natural light, ventilation, and enhancing the overall design of the building.",
        "Door installation involves fixing doors, providing security, access, and contributing to the building's architectural style.",
        "Cabinet installation is the process of placing storage cabinets in various rooms, optimizing space and organization.",
        "Appliance setup includes installing and configuring household appliances, ensuring they function correctly and efficiently.",
        "Landscaping design involves planning and creating an outdoor environment, incorporating greenery and aesthetic elements around the building.",
        "Fence installation encompasses erecting boundaries around the property, enhancing security and privacy.",
        "Driveway paving focuses on creating a paved surface for vehicles, enhancing accessibility and aesthetics.",
        "Home security system setup involves installing and configuring security systems to protect the building and its occupants.",
        "Smart home integration includes incorporating advanced technologies to enhance automation and connectivity within the building.",
        "Final inspection is the last step, involving a thorough examination of the completed building to ensure quality, safety, and adherence to standards."

        };

        string[] taskAliases =
        {
         "nothing",
        "Layout",
        "Frame",
        "Wire",
        "Plumb",
        "Cool",
        "Drywall",
        "Floor",
        "Roof",
        "Paint",
        "Decorate",
        "Window",
        "Door",
        "Cabinet",
        "Appliances",
        "Garden",
        "Fence",
        "Pave",
        "Secure",
        "Automate",
        "Inspect"
        };
        string[] TaskDeliverables =
         {
        "nothing",
        "Architectural Planning",
        "Structural Framing",
        "Electrical Wiring",
        "Plumbing Installation",
        "HVAC Systems Setup",
        "Drywall Installation",
        "Flooring Installation",
        "Roofing Installation",
        "Exterior Painting",
        "Interior Painting",
        "Window Installation",
        "Door Installation",
        "Cabinet Installation",
        "Appliance Setup",
        "Landscaping Design",
        "Fence Installation",
        "Driveway Paving",
        "Home Security System Setup",
        "Smart Home Integration",
        "Final Inspection"
         };
        for (int i = 1; i < 21; i++) 
        {
            DateTime start = new DateTime(2023, 1, i);
            int range = (DateTime.Today - start).Days;
            DateTime _createdAtDate = start.AddDays(i*10);
            DateTime _StartDate= _createdAtDate.AddDays(i*10 );
            DateTime _ScheduledDate = _createdAtDate.AddDays(i*10);
            DateTime _DeadlineDate = _ScheduledDate.AddDays(i + 50);
            DateTime _CompleteDate = _ScheduledDate.AddDays(2);
            TimeSpan _requiredEffortTime = _CompleteDate - _StartDate;
            int _numforenum = s_rand.Next(0, 5);
            EngineerExperience _engineerld = (EngineerExperience)_numforenum;
            Task newTask = new Task(0, taskAliases[i], TaskDescriptions[i],false,_createdAtDate,null,null,null,null, _requiredEffortTime, TaskDeliverables[i],null,null, _engineerld);
            s_dal!.Task.Create(newTask); //stage 2
        }
    }
    /// <summary>
    /// An operation that initializes entities of type Engineer with data
    /// </summary>
    private static void createEngineers()
    {
        string[] EngineerNames =
        {
        "Sara Borgen", "Naama Leah Radonsky","Ester Dray",
        "Ariela Levin", "Dina Klein", "Shira Israelof","Dani Levi"
        };
        string[] EngineerEmails =
        {
            "sara.borgen@emailgame.com",
            "naama.radonsky@emailgame.com",
            "ester.dray@emailgame.com",
            "ariela.levin@emailgame.com",
            "dina.klein@emailgame.com",
            "shira.israelof@emailgame.com",
            "dani.levi@emailgame.com"
        };

        for (int i = 0; i < EngineerNames.Length; i++)
        {
            int _id;
            do
            _id = s_rand.Next(200000000, 400000001);
            while (s_dal!.Engineer.Read(item => item.Id == _id)!= null);
            int _numforenum = s_rand.Next(0, 5);
            EngineerExperience _level = (EngineerExperience)_numforenum;
            int _cost;
            _cost = s_rand.Next(70, 301);

            Engineer newEngineer = new Engineer(_id, EngineerNames[i], EngineerEmails[i], _level, _cost);
            s_dal!.Engineer.Create(newEngineer);

        }

        Engineer newEngineer1 = new(123456789, "Eliezer El", "Eliezer@gmail.com", (EngineerExperience)2, 300);
        s_dal!.Engineer.Create(newEngineer1);
        Engineer newEngineer2 = new(987654321, "Shira Kehalani", "shira.ka017@gmail.com", (EngineerExperience)3, 301);
        s_dal!.Engineer!.Create(newEngineer2);
        Engineer newEngineer3 = new(654567898, "Tamar Chayat", "TAMARHAYAT1@gmail.com", (EngineerExperience)3, 301);
        s_dal!.Engineer.Create(newEngineer3);
    }

    /// <summary>
    /// An operation that initializes entities of type dependency with data
    /// </summary>
    private static void createDependencys()
{
    for (int i=2; i < 20; i++)
    {
        Dependency newDependency = new Dependency(0,i,i-1);
            s_dal!.Dependency.Create(newDependency);
    }
    for (int i = 4; i < 20; i++)
    {
        Dependency newDependency = new Dependency(0, i, 2);
            s_dal!.Dependency.Create(newDependency);
    }
    for (int i = 3; i < 20; i++)
    {
        Dependency newDependency = new Dependency(0, i, 2);
            s_dal!.Dependency.Create(newDependency);
    }
}
}
