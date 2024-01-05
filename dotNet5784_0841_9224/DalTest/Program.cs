using Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;

namespace DalTest
{
    internal class Program
    {
        private static ITask? s_dalTask = new TaskImplementation(); //stage 1
        private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
        private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1

        enum Menue {Exit, Task, Dependency,Engineer}
        enum SubMenue { Exit,Create,Read,ReadAll,Update,Delete}
        private static Menue menueM()
        {
            Console.WriteLine("Select an entity you want to check:\r\nFor a task tap 1\r\nFor dependencies press 2\r\nFor the engineer press 3\r\nTo exit the main program press 0");
            return (Menue)Console.Read(); 
        }
        private static SubMenue subMenueM()
        {
            Console.WriteLine("Select the method you want to perform:\r\nTo exit the main menu press 0\r\nTo add a new object of the entity type to the list tap 1\r\nTo display an object by ID, press 2\r\nTo display the list of all objects of the entity type press 3\r\nTo update the data of an existing object, press 4\r\nTo delete an existing object from the list, press 5");
            return  (SubMenue)Console.Read();
        }
        private static void taskCase()
        {
            SubMenue subMenue = subMenueM();
            if (subMenue != 0)
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
        private static void dependencyCase()
        {
            SubMenue subMenue = subMenueM();
            if (subMenue != 0)
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
        private static void engineerCase()
        {
            SubMenue subMenue = subMenueM();
            if (subMenue != 0)
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
            Task task = new Task(0, alies, description, mileston, createdAtDate, startDate, scheduledDate, deadlineDate, completeDate, requiredEffortTime, product, remarks, engineerID, difficulty);
            s_dalTask.Create(task);
        }
        private static void readTaskCase()
        {
            Console.WriteLine("Enter Id");
            int id=int.Parse(Console.ReadLine());
            Task task = s_dalTask.Read(id);
            Console.WriteLine(task.ToString());
        }
        private static void readAllTaskCase()
        {
            List<Task> tasks = s_dalTask.ReadAll();
            foreach (Task item in tasks)
            {
                Console.WriteLine(item.ToString());
            }
        }
        private static void updateTaskCase()
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine());
            Task task = s_dalTask.Read(id);

            Console.WriteLine("Enter alies");
            string alies = Console.ReadLine();
            if( alies == null) { alies = task.alies; }
            Console.WriteLine("Enter description");
            string description = Console.ReadLine();
            if (description == null) {  description = task.description; }
            Console.WriteLine("Enter milestone");
            string tempMilesone = Console.ReadLine();
            if (tempMilesone == null) { tempMilesone = task.tempMilesone; }
            bool mileston = (tempMilesone == "true") ? true : false;
            Console.WriteLine("Enter task creation date");
            if () ;
            DateTime? createdAtDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter planned date for the start of work");
            if () ;
            DateTime? startDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter date of commencement of work on the assignment");
            if () ;
            DateTime? scheduledDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter deadline");
            if () ;
            DateTime? deadlineDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter actual end date");
            if () ;
            DateTime? completeDate = DateTime.Parse(Console.ReadLine());
            TimeSpan? requiredEffortTime = deadlineDate - startDate;
            Console.WriteLine("Enter product");
            string product = Console.ReadLine();
            if(product==null) { product = task.product; }
            Console.WriteLine("Enter remarks");
            string remarks = Console.ReadLine();
            if (remarks == null) {  remarks = task.remarks; }
            Console.WriteLine("Enter the engineer ID assigned to the task");
            int engineerID = int.Parse(Console.ReadLine());
            if (engineerID == 0) { engineerID = task.engineerID; }
            Console.WriteLine("Enter numer of the difficulty level of the task");
            int difficultyNumber = int.Parse(Console.ReadLine());
            EngineerExperience difficulty = (EngineerExperience)difficultyNumber;
            Task task = new Task(0, alies, description, mileston, createdAtDate, startDate, scheduledDate, deadlineDate, completeDate, requiredEffortTime, product, remarks, engineerID, difficulty);
            s_dalTask.Create(task);

        }
        private static void deleteTaskCase()
        {
            Console.WriteLine("Enter id");
            int id = int.Parse(Console.ReadLine());
            try
            {
                s_dalTask.Delete(id);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }  
        }
        private static void createDependencyCase()
        {
            Console.WriteLine("Enter an ID number of a previous task");
            int dependentTask = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter a pending task ID number");
            int dependentOnTask = int.Parse(Console.ReadLine());
            Dependency dependency = new Dependency(0, dependentTask, dependentOnTask);
            s_dalDependency.Create(dependency);
        }
        private static void readDependencyCase()
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine());
            Dependency dependency = s_dalDependency.Read(id);
            Console.WriteLine(dependency.ToString());

        }
        private static void readAllDependencyCase()
        {
            List<Dependency> dependencys = s_dalDependency.ReadAll();
            foreach (Dependency item in dependencys)
            {
                Console.WriteLine(item.ToString());
            }
        }
        private static void updateDependencyCase()
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine());
            Dependency dependency = s_dalDependency.Read(id);
            if (dependency != null)
            {
                Console.WriteLine(dependency.ToString());
            }

            Console.WriteLine("Enter an ID number of a previous task");
            int dependentTask = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter a pending task ID number");
            int dependentOnTask = int.Parse(Console.ReadLine());
            if(dependentTask == 0) { dependentTask = dependency.DependentTask; }
            if(dependentOnTask == 0) { dependentOnTask = dependency.DependentOnTask; }
            Dependency dependency2 = new Dependency(0, dependentTask, dependentOnTask);
            s_dalDependency.Create(dependency2);
        }
        private static void deleteDependencyCase()
        {
            Console.WriteLine("Enter id");
            int id = int.Parse(Console.ReadLine());
            try
            {
                s_dalDependency.Delete(id);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
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
            s_dalEngineer.Create(engineer);
        }
        private static void readEngineerCase()
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine());
            Engineer engineer = s_dalEngineer.Read(id);
            Console.WriteLine(engineer.ToString());
        }
        private static void readAllEngineerCase()
        {
            List<Engineer> engineers = s_dalEngineer.ReadAll();
            foreach (Engineer item in engineers)
            {
                Console.WriteLine(item.ToString());
            }
        }
        private static void updateEngineerCase()
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine());
            Engineer engineer = s_dalEngineer.Read(id);
            Console.WriteLine(engineer.ToString());
            
            Console.WriteLine("Enter the name of the engineer (full name)");
            string name = Console.ReadLine();
            Console.WriteLine("Enter an email address");
            string email = Console.ReadLine();
            Console.WriteLine("Enter the level of the engineer");
            int difficultyNumber = int.Parse(Console.ReadLine());
            EngineerExperience difficulty = (EngineerExperience)difficultyNumber;
            Console.WriteLine("Enter an hourly cost");
            double cost = double.Parse(Console.ReadLine());
            if(name==null) { name=engineer.Name; }
            if(email==null) { email=engineer.Email; }
            if (difficulty==null) { difficulty = engineer.Level; }
            if (cost == null) {cost=engineer.Cost; }
            Engineer engineer2 = new Engineer(id, name, email, difficulty, cost);
            s_dalEngineer.Create(engineer2);
        }
    
        private static void deleteEngineerCase()
        {
            Console.WriteLine("Enter id");
            int id =int.Parse(Console.ReadLine());
            try
            {
                s_dalEngineer.Delete(id);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); } 
        }

        static void Main(string[] args)
        {
            try
            {
                Menue menue = menueM();
                while (menue!=0)
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
                    menue = menueM();
                }
            }        
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
