using Dal;
using DalApi;
using DO;
using System.Xml.Linq;
using static Dal.XMLTools;

namespace DalTest
{
    internal class Program
    {
        //static readonly IDal s_dal = new DalList(); //stage 2
      static readonly IDal s_dal = new DalXml(); //stage 3
        /// <summary>
        /// A function that displays a main menu and captures the selection of the variable
        /// </summary>
        enum Menue {Exit, Task, Dependency,Engineer};
        /// <summary>
        /// A function that displays a submenu for the entities and captures the user's selection.
        /// </summary>
        enum SubMenue { Exit,Create,Read,ReadAll,Update,Delete};

        private static void getAndIncreaseNextId(string elemName)
        {
            XElement root = LoadListFromXMLElement("data-config");
            root.Element(elemName)?.SetValue(1);
            SaveListToXMLElement(root, "data-config");
        }

        private static void reset() 
        {
            IEnumerable<DO.Task> tasks = s_dal.Task.ReadAll();

            foreach (DO.Task item in tasks)
                s_dal.Task.Delete(item.Id);

            IEnumerable<DO.Dependency> dependencys = s_dal.Dependency.ReadAll();

            foreach (Dependency item in dependencys)
                s_dal.Dependency.Delete(item.Id);

            IEnumerable<DO.Engineer> engineers = s_dal.Engineer.ReadAll();

            foreach (Engineer item in engineers)
                s_dal.Engineer.Delete(item.Id);

            getAndIncreaseNextId("NextTaskId");
            getAndIncreaseNextId("NextDependencyId");
        }
        /// <summary>
        /// Print main menu
        /// </summary>
        private static void menueM()
        {
            Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
            if (ans == "Y") //stage 3
            {
                reset();
                Initialization.Do(s_dal); //stage 2
            }
            Console.WriteLine("Select an entity you want to check:\r\nFor a task tap 1\r\nFor dependencies press 2\r\nFor the engineer press 3\r\nTo exit the main program press 0");
            return;  
        }
        /// <summary>
        /// Print submenu
        /// </summary>
        private static void subMenueM()
        {
            Console.WriteLine("Select the method you want to perform:\r\nTo exit the main menu press 0\r\nTo add a new object of the entity type to the list tap 1\r\nTo display an object by ID, press 2\r\nTo display the list of all objects of the entity type press 3\r\nTo update the data of an existing object, press 4\r\nTo delete an existing object from the list, press 5");
            return;
        }
        /// <summary>
        /// Submenu activation for a task entity
        /// </summary>
        private static void taskCase()
        {
            SubMenue subMenue;
            subMenueM();
            string temp = Console.ReadLine();
            if (Enum.TryParse(temp, out subMenue) && subMenue != SubMenue.Exit)
            {
                switch (subMenue)
                {
                    case SubMenue.Create:
                        createTaskCase();
                        break;
                    case SubMenue.Read:
                        readTaskCase();
                        break;
                    case SubMenue.ReadAll:
                        readAllTaskCase();
                        break;
                    case SubMenue.Update:
                        updateTaskCase();
                        break;
                    case SubMenue.Delete:
                        deleteTaskCase();
                        break;
                    default:
                        break;
                }
            }
        }

        private void printList<Item>(IEnumerable<Item> items)
            => Console.WriteLine(string.Join(Environment.NewLine, items));

        /// <summary>
        /// Submenu activation for a dependency entity
        /// </summary>
        private static void dependencyCase()
        {
            SubMenue subMenue;
            subMenueM();
            string temp = Console.ReadLine();
            if (Enum.TryParse(temp, out subMenue) && subMenue != SubMenue.Exit)
            {
                switch (subMenue)
                {
                    case SubMenue.Create:
                        createDependencyCase();
                        break;
                    case SubMenue.Read:
                        readDependencyCase();
                        break;
                    case SubMenue.ReadAll:
                        readAllDependencyCase();
                        break;
                    case SubMenue.Update:
                        updateDependencyCase();
                        break;
                    case SubMenue.Delete:
                        deleteDependencyCase();
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Submenu activation for a engineer entity
        /// </summary>
        private static void engineerCase()
        {
            SubMenue subMenue;
            subMenueM();
            string temp = Console.ReadLine();
            if (Enum.TryParse(temp, out subMenue) && subMenue != SubMenue.Exit)
            {
                switch (subMenue)
                {
                    case SubMenue.Create:
                        createEngineerCase();
                        break;
                    case SubMenue.Read:
                        readEngineerCase();
                        break;
                    case SubMenue.ReadAll:
                        readAllEngineerCase();
                        break;
                    case SubMenue.Update:
                        updateEngineerCase();
                        break;
                    case SubMenue.Delete:
                        deleteEngineerCase();
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Creating a  new task entity that includes receiving the variables and saving them
        /// </summary>
        private static void createTaskCase ()
        {
            Console.WriteLine("Enter alies");
            string alies = Console.ReadLine();
            Console.WriteLine("Enter description");
            string description = Console.ReadLine();
            Console.WriteLine("Enter milestone");
            string tempMilesone = Console.ReadLine();
            bool mileston = (tempMilesone == "true") ? true : false;
            Console.WriteLine("Enter task creation date");
            DateTime? createdAtDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter planned date for the start of work");
            DateTime? startDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter date of commencement of work on the assignment");
            DateTime? scheduledDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter deadline");
            DateTime? deadlineDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter actual end date");
            DateTime? completeDate = DateTime.Parse(Console.ReadLine());
            TimeSpan? requiredEffortTime = deadlineDate - startDate;
            Console.WriteLine("Enter product");
            string product = Console.ReadLine();
            Console.WriteLine("Enter remarks");
            string remarks = Console.ReadLine();
            Console.WriteLine("Enter the engineer ID assigned to the task");
            int engineerID = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter numer of the difficulty level of the task");
            int difficultyNumber = int.Parse(Console.ReadLine());
            EngineerExperience difficulty = (EngineerExperience)difficultyNumber;
            DO.Task task = new(0, alies, description, mileston, createdAtDate, startDate, scheduledDate, deadlineDate, completeDate, requiredEffortTime, product, remarks, engineerID, difficulty);
            s_dal.Task.Create(task);
        }
        /// <summary>
        /// Displaying the details of a certain task according to a certain id
        /// </summary>
        private static void readTaskCase()
        {
            Console.WriteLine("Enter Id");
            int id=int.Parse(Console.ReadLine());
            DO.Task task = s_dal.Task.Read(item => item.Id == id);
            Console.WriteLine(task.ToString());
        }
        /// <summary>
        /// Displaying all the details of all the tasks that exist in the database
        /// </summary>
        private static void readAllTaskCase()
        {
            IEnumerable<DO.Task?> tasks = s_dal.Task.ReadAll();
            foreach (DO.Task item in tasks)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        /// Updating the details of a certain task according to the received id, which includes the reception of new details
        /// </summary>
        private static void updateTaskCase()
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine());
            DO.Task task = s_dal.Task.Read(item => item.Id == id);

            Console.WriteLine("Enter alies");
            string alies = Console.ReadLine();
            if( alies == null) { alies = task.Ailas; }
            Console.WriteLine("Enter description");
            string description = Console.ReadLine();
            if (description == null) {  description = task.Description; }
            Console.WriteLine("Enter milestone");
            string tempMilesone = Console.ReadLine();
            bool mileston = (tempMilesone == "true") ? true : false;
            if (tempMilesone == null) { mileston = task.IsMilestone; }
            Console.WriteLine("Enter task creation date");
            DateTime? createdAtDate = DateTime.Parse(Console.ReadLine());
            if (createdAtDate == null) { createdAtDate = task.CreatedAtDate; } 
            Console.WriteLine("Enter planned date for the start of work");
            DateTime? startDate = DateTime.Parse(Console.ReadLine());
            if (startDate == null) { startDate = task.StartDate; }
            Console.WriteLine("Enter date of commencement of work on the assignment");
            DateTime? scheduledDate = DateTime.Parse(Console.ReadLine());
            if (scheduledDate == null) { scheduledDate = task.ScheduledDate; }
            Console.WriteLine("Enter deadline");
            DateTime? deadlineDate = DateTime.Parse(Console.ReadLine());
           if (deadlineDate == null) { deadlineDate = task.DeadlineDate; }
            Console.WriteLine("Enter actual end date");
            DateTime? completeDate = DateTime.Parse(Console.ReadLine());
            if (completeDate == null) { completeDate = task.CompleteDate; }
            TimeSpan? requiredEffortTime = deadlineDate - startDate;
            Console.WriteLine("Enter product");
            string product = Console.ReadLine();
            if(product==null) { product = task.Deliverables; }
            Console.WriteLine("Enter remarks");
            string remarks = Console.ReadLine();
            if (remarks == null) {  remarks = task.Remarks; }
            Console.WriteLine("Enter the engineer ID assigned to the task");
            int? engineerID = int.Parse(Console.ReadLine());
            if (engineerID == 0) { engineerID = task.Engineerld; }
            Console.WriteLine("Enter numer of the difficulty level of the task");
            int difficultyNumber = int.Parse(Console.ReadLine());
            EngineerExperience difficulty = (EngineerExperience)difficultyNumber;
            if (difficulty == null) { difficulty = task.Copmlexity.Value; }
            DO.Task task1 = new(id, alies, description, mileston, createdAtDate, startDate, scheduledDate, deadlineDate, completeDate, requiredEffortTime, product, remarks, engineerID, difficulty);
            try
            {
                s_dal.Task.Update(task1);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
           

        }
        /// <summary>
        /// Deleting a task entity according to the received id
        /// </summary>
        private static void deleteTaskCase()
        {
            Console.WriteLine("Enter id");
            int id = int.Parse(Console.ReadLine());
            try
            {
                s_dal.Task.Delete(id);
            }
             catch (Exception ex) { Console.WriteLine(ex.ToString()); }  
        }
        /// <summary>
        /// Creating a new dependency entity that includes receiving the variables and saving them
        /// </summary>
        private static void createDependencyCase()
        {
            Console.WriteLine("Enter an ID number of a previous task");
            int dependentTask = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter a pending task ID number");
            int dependentOnTask = int.Parse(Console.ReadLine());
            Dependency dependency = new Dependency(0, dependentTask, dependentOnTask);
            s_dal.Dependency.Create(dependency);
        }
        /// <summary>
        /// Displaying the details of a certain dependency according to a certain id
        /// </summary>
        private static void readDependencyCase()
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine());
            Dependency dependency = s_dal.Dependency.Read(item => item.Id == id);
            Console.WriteLine(dependency.ToString());

        }
        /// <summary>
        /// Displaying all the details of all the dependencys that exist in the database
        /// </summary>
        private static void readAllDependencyCase()
        {
            IEnumerable<Dependency?> dependencys = s_dal.Dependency.ReadAll();
            foreach (Dependency item in dependencys)
            {
                Console.WriteLine(item.ToString());
            }
        }
        /// <summary>
        /// A function that calls a function by an ID number of the dependency to update details in the dependency.
        /// </summary>
        private static void updateDependencyCase()
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine());
            Dependency dependency = s_dal.Dependency.Read(item => item.Id == id);
            Console.WriteLine(dependency.ToString());
            

            Console.WriteLine("Enter an ID number of a previous task");
            int? dependentTask = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter a pending task ID number");
            int? dependentOnTask = int.Parse(Console.ReadLine());
            if(dependentTask == 0) { dependentTask =dependency.DependentTask; }
            if(dependentOnTask == 0) { dependentOnTask = dependency.DependentOnTask; }
            Dependency dependency2 = new Dependency(0, dependentTask, dependentOnTask);
            try 
            {
                s_dal.Dependency.Update(dependency2);
            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// A function that calls a delete function for an existing dependency
        /// </summary>
        private static void deleteDependencyCase()
        {
            Console.WriteLine("Enter id");
            int id = int.Parse(Console.ReadLine());
            try
            {
                s_dal.Dependency.Delete(id);
            }   
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        /// <summary>
        /// A function that calls an initialization function for a new engineer with all his details
        /// </summary>
        private static void createEngineerCase()
        {
            Console.WriteLine("Enter a unique ID number");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the name of the engineer (full name)");
            string name = Console.ReadLine();
            Console.WriteLine("Enter an email address");
            string email = Console.ReadLine();
            Console.WriteLine("Enter the level of the engineer");
            int difficultyNumber = int.Parse(Console.ReadLine());
            EngineerExperience difficulty = (EngineerExperience)difficultyNumber;
            Console.WriteLine("Enter an hourly cost");
            double cost = double.Parse(Console.ReadLine());
            Engineer engineer = new Engineer(id,name,email, difficulty, cost);
            s_dal.Engineer.Create(engineer);
        }
        /// <summary>
        /// A function that orders a print function for a specific engineer - according to his thesis
        /// </summary>
        private static void readEngineerCase()
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine());
            Engineer engineer = s_dal.Engineer.Read(item => item.Id == id);
            Console.WriteLine(engineer.ToString());
        }
        /// <summary>
        /// A function that calls a print function for a list of existing engineers with all the data in it
        /// </summary>
        private static void readAllEngineerCase()
        {
            IEnumerable<Engineer?> engineers = s_dal.Engineer.ReadAll();
            foreach (Engineer item in engineers)
            {
                Console.WriteLine(item.ToString());
            }
        }
        /// <summary>
        /// A function that calls a function to update data for an existing engineer. If the user entered a null variable the function will leave the previous value of the engineer
        /// </summary>
        private static void updateEngineerCase()
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine());

            Engineer engineer = s_dal.Engineer.Read(item => item.Id == id);
            Console.WriteLine(engineer.ToString());
            
            Console.WriteLine("Enter the name of the engineer (full name)");
            string name = Console.ReadLine();
            Console.WriteLine("Enter an email address");
            string email = Console.ReadLine();
            Console.WriteLine("Enter the level of the engineer");
            int difficultyNumber = int.Parse(Console.ReadLine());
            EngineerExperience? difficulty = (EngineerExperience)difficultyNumber;
            Console.WriteLine("Enter an hourly cost");
            double? cost = double.Parse(Console.ReadLine());
            if(name==null) { name=engineer.Name; }
            if(email==null) { email=engineer.Email; }
            if (difficulty==null) { difficulty = engineer.Level; }
            if (cost == null) {cost=engineer.Cost; }
            engineer = new Engineer(id, name, email, difficulty, cost);
            try
            {
                s_dal.Engineer.Update(engineer);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }
        /// <summary>
        /// A function that activates delete that deletes an engineer from the list of engineers according to a desired Id
        /// </summary>
        private static void deleteEngineerCase()
        {
            Console.WriteLine("Enter id");
            int id =int.Parse(Console.ReadLine());
            try
            {
                s_dal.Engineer.Delete(id);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); } 
        }
        /// <summary>
        /// A main program that checks the integrity of the program and the entities that exist in it
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
             //  Initialization.Do(s_dal); //stage 2
                Menue menue;
                menueM();
                string userInput = Console.ReadLine();
                while (Enum.TryParse(userInput, out menue) && menue != Menue.Exit)    
                {
                    switch (menue)
                    {
                        case Menue.Task:
                            taskCase();
                            break;
                        case Menue.Dependency:
                            dependencyCase();
                                break;
                        case Menue.Engineer:
                            engineerCase();
                                break;
                        default:
                            break;
                    }
                    menueM();
                    userInput = Console.ReadLine();
                }
            }        
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
