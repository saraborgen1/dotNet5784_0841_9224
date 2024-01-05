using Dal;
using DalApi;
using DO;
using System.Diagnostics.Metrics;

namespace DalTest
{
    internal class Program
    {
        private static ITask? s_dalTask = new TaskImplementation(); //stage 1
        private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
        private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1

        enum Menue { Exit, Task, Dependency, Engineer }
        enum SubMenue { Exit, Create, Read, ReadAll, Update, Delete }
        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalTask, s_dalDependency, s_dalEngineer);
                Console.WriteLine("Select an entity you want to check:\r\nFor a task tap 1\r\nFor dependencies press 2\r\nFor the engineer press 3\r\nTo exit the main program press 0");
                Menue menue;
                menue = (Menue)Console.Read();
                SubMenue subMenue;
                while (menue != 0)
                {
                    switch (menue)
                    {
                        case Menue.Task:
                            Console.WriteLine("Select the method you want to perform:\r\nTo exit the main menu press 0\r\nTo add a new object of the entity type to the list tap 1\r\nTo display an object by ID, press 2\r\nTo display the list of all objects of the entity type press 3\r\nTo update the data of an existing object, press 4\r\nTo delete an existing object from the list, press 5");
                            subMenue = (SubMenue)Console.Read();
                            if (subMenue != 0)
                            {
                                switch (subMenue)
                                {
                                    case SubMenue.Create:
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
                                        Task task = new Task(0, alies, description, mileston, createdAtDate, startDate, scheduledDate, deadlineDate, completeDate, requiredEffortTime, product, remarks, engineerID, difficulty);

                                        Console.WriteLine("Enter");
                                        Console.WriteLine("Enter");
                                        Console.WriteLine("Enter");
                                        Console.WriteLine("Enter");
                                        Console.WriteLine("Enter");
                                        break;
                                    case SubMenue.Read:
                                        break;
                                    case SubMenue.ReadAll:
                                        break;
                                    case SubMenue.Update:
                                        break;
                                    case SubMenue.Delete:
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case Menue.Dependency:

                            Console.WriteLine("Select the method you want to perform:\r\nTo exit the main menu press 0\r\nTo add a new object of the entity type to the list tap 1\r\nTo display an object by ID, press 2\r\nTo display the list of all objects of the entity type press 3\r\nTo update the data of an existing object, press 4\r\nTo delete an existing object from the list, press 5");
                            subMenue = (SubMenue)Console.Read();
                            if (subMenue != 0)
                            {
                                switch (subMenue)
                                {
                                    case SubMenue.Create:
                                        Console.WriteLine("Enter an ID number of a previous task");
                                        int dependentTask = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter a pending task ID number");
                                        int dependentOnTask = int.Parse(Console.ReadLine());
                                        Dependency dependency = new Dependency(0, dependentTask, dependentOnTask);
                                        break;
                                    case SubMenue.Read:
                                        break;
                                    case SubMenue.ReadAll:
                                        break;
                                    case SubMenue.Update:
                                        break;
                                    case SubMenue.Delete:
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case Menue.Engineer:

                            Console.WriteLine("Select the method you want to perform:\r\nTo exit the main menu press 0\r\nTo add a new object of the entity type to the list tap 1\r\nTo display an object by ID, press 2\r\nTo display the list of all objects of the entity type press 3\r\nTo update the data of an existing object, press 4\r\nTo delete an existing object from the list, press 5");
                            subMenue = (SubMenue)Console.Read();
                            if (subMenue != 0)
                            {
                                switch (subMenue)
                                {
                                    case SubMenue.Create:
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
                                        int cost = int.Parse(Console.ReadLine());

                                        break;
                                    case SubMenue.Read:
                                        break;
                                    case SubMenue.ReadAll:
                                        break;
                                    case SubMenue.Update:
                                        break;
                                    case SubMenue.Delete:
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
