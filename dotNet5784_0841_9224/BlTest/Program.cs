using DO;
using System.Xml.Linq;
using static BO.Enums;
using static Dal.XMLTools;

namespace BlTest
{
    internal class Program
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private static bool change(string name)
        {
            Console.WriteLine($"if you would like to update the {name} enter 1 if not enter 0 \n");
            var temp = int.TryParse(Console.ReadLine(), out int num);
            while (!temp || (num != 0 && num != 1))
            {
                Console.WriteLine($" you idiot , wht cant you read instructions ,I told you if you would like to update the {name} enter 1 if not enter 0 \n");
                temp = int.TryParse(Console.ReadLine(), out num);
            }
            if (num == 0)
            {
                Console.WriteLine($"enter {name}");
                return true;
            }
            return false;
        }
        /// <summary>
        /// A function that displays a main menu and captures the selection of the variable
        /// </summary>
        enum Menue { Exit, Task, Engineer, Time };

        /// <summary>
        /// A function that displays a submenu for the entities and captures the user's selection.
        /// </summary>
        enum SubMenue { Exit, Create, Read, ReadAll, Delete, Update, UpdateDate };

        // <summary>
        /// Reset the running number
        /// </summary>
        /// <param name="elemName">name of running number</param>
        private static void getAndIncreaseNextId(string elemName)
        {
            XElement root = LoadListFromXMLElement("data-config");
            root.Element(elemName)?.SetValue(1);
            SaveListToXMLElement(root, "data-config");
        }

        /// <summary>
        /// Deletes the entities and resets the running numbers
        /// </summary>
        private static void reset()
        {
            IEnumerable<BO.Task> tasks = s_bl.Task.ReadAll().ToList();

            foreach (BO.Task item in tasks)
                s_bl.Task.Delete(item.Id);


            IEnumerable<BO.Engineer> engineers = s_bl.Engineer.ReadAll().ToList();

            foreach (BO.Engineer item in engineers)
                s_bl.Engineer.Delete(item.Id);

            getAndIncreaseNextId("NextTaskId");
            getAndIncreaseNextId("NextDependencyId");
        }

        /// <summary>
        /// Print main menu
        /// </summary>
        private static void menueM()
        {
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
            {
                reset();
                DalTest.Initialization.Do();
            }

            Console.WriteLine("Select an entity you want to check:\r\nFor a task tap 1\r\nFor the engineer press 2\r\nFor enter hours for tasks in the project\r\n To exit the main program press 0");
        }

        /// <summary>
        /// Print submenu
        /// </summary>
        private static void subMenueM()
        {
            Console.WriteLine("Select the method you want to perform:\r\nTo exit the main menu press 0\r\nTo add a new object of the entity type to the list tap 1\r\nTo display an object by ID, press 2\r\nTo display the list of all objects of the entity type press 3\r\nTo delete an existing object from the list, press 4\r\n To update the data of an existing object, press 5\r\nTo update the start date of a task, press 6\r\n");
        }
        private static void subMenueMEngineer()
        {
            Console.WriteLine("Select the method you want to perform:\r\nTo exit the main menu press 0\r\nTo add a new object of the entity type to the list tap 1\r\nTo display an object by ID, press 2\r\nTo display the list of all objects of the entity type press 3\r\nTo delete an existing object from the list, press 4\r\n To update the data of an existing object, press 5\r\n");
        }

        private static void subMenueMTime()
        {
            Console.WriteLine();
        }
        /// <summary>
        /// Submenu activation for a task entity
        /// </summary>
        private static void taskCase()
        {
            SubMenue subMenue;
            subMenueM();
            string temp = Console.ReadLine()!;
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
                    case SubMenue.Delete:
                        deleteTaskCase();
                        break;
                    case SubMenue.Update:
                        updateTaskCase();
                        break;
                    case SubMenue.UpdateDate:
                        updateDateTaskCase();
                        break;
                    default:
                        break;
                }
            }
        }

        private static void timeCase()
        {
            dje
        }


        /// <summary>
        /// Submenu activation for a engineer entity
        /// </summary>
        private static void engineerCase()
        {
            SubMenue subMenue;
            subMenueMEngineer();
            string temp = Console.ReadLine()!;

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
                    case SubMenue.Delete:
                        deleteEngineerCase();
                        break;
                    case SubMenue.Update:
                        updateEngineerCase();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Creating a  new task entity that includes receiving the variables and saving them
        /// </summary>
        private static void createTaskCase()
        {
            Console.WriteLine("Enter alies");
            string alies = Console.ReadLine()!;

            Console.WriteLine("Enter description");
            string description = Console.ReadLine()!;

            List<BO.TaskInList>? dependencies = null;
            Console.WriteLine("Enter a list of id of dependencies. When you are done, enter 0");
            int num = int.Parse(Console.ReadLine()!);
            while (num != 0)
            {
                dependencies!.Add(new BO.TaskInList() { Id = num, Alias = null, Description = null, Status = 0 });
                num = int.Parse(Console.ReadLine()!);
            }

            Console.WriteLine("Enter required Effort Time");
            bool temp = TimeSpan.TryParse(Console.ReadLine(), out TimeSpan requiredEffortTime);
            while (!temp)
            {
                Console.WriteLine("ERROR,Enter required Effort Time");
                temp = TimeSpan.TryParse(Console.ReadLine(), out requiredEffortTime);
            }

            Console.WriteLine("Enter deliverables");
            string deliverables = Console.ReadLine()!;

            Console.WriteLine("Enter remarks");
            string remarks = Console.ReadLine()!;

            Console.WriteLine("Enter number of the difficulty level of the task between 0-4");
            temp = int.TryParse(Console.ReadLine(), out int difficultyNumber);
            while (!temp || difficultyNumber < 0 || difficultyNumber > 4)
            {
                Console.WriteLine("ERROR,Enter number of the difficulty level of the task between 0-4");
                temp = int.TryParse(Console.ReadLine(), out difficultyNumber);
            }
            BO.Enums.EngineerExperience difficulty = (BO.Enums.EngineerExperience)difficultyNumber;

            BO.Task task = new()
            {
                Id = 0,
                Alias = alies,
                Description = description,
                CreatedAtDate = null,
                Status = Status.Unscheduled,
                Dependencies = dependencies,
                RequiredEffortTime = requiredEffortTime,
                StartDate = null,
                ScheduledDate = null,
                ForecastDate = null,
                DeadlineDate = null,
                CompleteDate = null,
                Deliverables = deliverables,
                Remarks = remarks,
                Engineer = null,
                Copmlexity = difficulty
            };
            s_bl.Task.Create(task);
        }

        /// <summary>
        /// Displaying the details of a certain task according to a certain id
        /// </summary>
        private static void readTaskCase()
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine()!);

            BO.Task task = s_bl.Task.Read(id);
            Console.WriteLine(task.ToString());
        }

        /// <summary>
        /// Displaying all the details of all the tasks that exist in the database
        /// </summary>
        private static void readAllTaskCase()
        {
            IEnumerable<BO.Task> tasks = s_bl.Task.ReadAll();

            foreach (BO.Task item in tasks)
                Console.WriteLine(item);
        }

        /// <summary>
        /// Updating the details of a certain task according to the received id, which includes the reception of new details
        /// </summary>
        private static void updateTaskCase()
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine()!);

            BO.Task task = s_bl.Task.Read(id);
            string? alias = task.Alias;
            if (change("Alias"))
                alias = Console.ReadLine()!;

            string? description = task.Description;
            if (change("Description"))
                description = Console.ReadLine()!;
            
            DateTime? createdAtDate = task.CreatedAtDate;

            BO.Enums.Status status = task.Status;
            if (change("Status"))
            {
                bool temp = int.TryParse(Console.ReadLine(), out int difficultyNumber);
                while (!temp || difficultyNumber < 0 || difficultyNumber > 3)
                {
                    Console.WriteLine("ERROR,Enter number of the difficulty level of the task between 0-3");
                    temp = int.TryParse(Console.ReadLine(), out difficultyNumber);
                }
                BO.Enums.EngineerExperience difficulty = (BO.Enums.EngineerExperience)difficultyNumber;
            }
            List<BO.TaskInList>? dependencies = task.Dependencies;
            if (change("Dependencies"))
            {
                int num = int.Parse(Console.ReadLine()!);
                while (num != 0)
                {
                    dependencies!.Add(new BO.TaskInList() { Id = num, Alias = null, Description = null, Status = 0 });
                    num = int.Parse(Console.ReadLine()!);
                }
            }
            TimeSpan? requiredEffortTime = task.RequiredEffortTime;
            if (change("RequiredEffortTime"))
            {
                bool temp = TimeSpan.TryParse(Console.ReadLine(), out TimeSpan tempRequiredEffortTime);
                while (!temp)
                {
                    Console.WriteLine("ERROR,Enter required Effort Time");
                    temp = TimeSpan.TryParse(Console.ReadLine(), out tempRequiredEffortTime);
                }
                requiredEffortTime = tempRequiredEffortTime;
            }

            DateTime? startDate = task.StartDate;
            if (change("StartDate"))
            {
                bool temp = DateTime.TryParse(Console.ReadLine(), out DateTime tempStartDate);
                while (!temp)
                {
                    Console.WriteLine("ERROR,Enter StartDate Time");
                    temp = DateTime.TryParse(Console.ReadLine(), out tempStartDate);
                }
                startDate = tempStartDate;
            }

            DateTime? scheduledDate = task.ScheduledDate;
            if (change("ScheduledDate"))
            {
                bool temp = DateTime.TryParse(Console.ReadLine(), out DateTime tempScheduledDate);
                while (!temp)
                {
                    Console.WriteLine("ERROR,Enter StartDate Time");
                    temp = DateTime.TryParse(Console.ReadLine(), out tempScheduledDate);
                }
                scheduledDate = tempScheduledDate;
            }
            DateTime? forecastDate = task.ForecastDate;
            if (change("ForecastDate"))
            {
                bool temp = DateTime.TryParse(Console.ReadLine(), out DateTime tempForecastDate);
                while (!temp)
                {
                    Console.WriteLine("ERROR,Enter ForecastDate Time");
                    temp = DateTime.TryParse(Console.ReadLine(), out tempForecastDate);
                }
                forecastDate = tempForecastDate;
            }

            DateTime? deadlineDate = task.DeadlineDate;
            if (change("DeadlineDate"))
            {
                bool temp = DateTime.TryParse(Console.ReadLine(), out DateTime tempDeadlineDate);
                while (!temp)
                {
                    Console.WriteLine("ERROR,Enter ForecastDate Time");
                    temp = DateTime.TryParse(Console.ReadLine(), out tempDeadlineDate);
                }
                deadlineDate = tempDeadlineDate;
            }

            DateTime? completeDate = task.CompleteDate;
            if (change("CompleteDate"))
            {
                bool temp = DateTime.TryParse(Console.ReadLine(), out DateTime tempCompleteDate);
                while (!temp)
                {
                    Console.WriteLine("ERROR,Enter ForecastDate Time");
                    temp = DateTime.TryParse(Console.ReadLine(), out tempCompleteDate);
                }
                completeDate = tempCompleteDate;
            }

            string? deliverables = task.Deliverables;
            if (change("Deliverables"))
            {
                deliverables = Console.ReadLine()!;
            }

            string? remarks = task.Remarks;
            if (change("Remarks"))
            {
                remarks = Console.ReadLine()!;
            }
            BO.EngineerInTask? engineer = task.Engineer;
            if (change("Engineer"))
            {
                dasd
            }
            BO.Enums.EngineerExperience copmlexity = task.Copmlexity;
            if (change("Copmlexity"))
            {
                gdgf
            }

            DO.Task task1 = new(id, alies, description, createdAtDate, startDate, scheduledDate, deadlineDate, completeDate, requiredEffortTime, product, remarks, engineerID, difficulty);
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
            int id = int.Parse(Console.ReadLine()!);
            try
            {
                s_bl.Task.Delete(id);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// A function that calls an initialization function for a new engineer with all his details
        /// </summary>
        private static void createEngineerCase()
        {
            Console.WriteLine("Enter a unique ID number");
            int id = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter the name of the engineer (full name)");
            string name = Console.ReadLine()!;

            Console.WriteLine("Enter an email address");
            string email = Console.ReadLine()!;

            Console.WriteLine("Enter the level of the engineer");
            int difficultyNumber = int.Parse(Console.ReadLine()!);
            BO.Enums.EngineerExperience difficulty = (BO.Enums.EngineerExperience)difficultyNumber;

            Console.WriteLine("Enter an hourly cost");
            double cost = double.Parse(Console.ReadLine()!);

            BO.Engineer engineer = new() { Id = id, Name = name, Email = email, Level = difficulty, Cost = cost, Task = null };
            s_bl.Engineer.Create(engineer);
        }
        /// <summary>
        /// A function that orders a print function for a specific engineer - according to his thesis
        /// </summary>
        private static void readEngineerCase()
        {
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine()!);
            BO.Engineer engineer = s_bl.Engineer.Read(id)!;
            Console.WriteLine(engineer.ToString());
        }
        /// <summary>
        /// A function that calls a print function for a list of existing engineers with all the data in it
        /// </summary>
        private static void readAllEngineerCase()
        {
            IEnumerable<BO.Engineer> engineers = s_bl.Engineer.ReadAll();
            foreach (BO.Engineer item in engineers)
            {
                Console.WriteLine(item.ToString());
            }
        }
        /// <summary>
        /// A function that calls a print function for a list of existing engineers with all the data in it
        /// </summary>
        private static void readAllDeletedEngineerCase()
        {
            IEnumerable<Engineer> engineers = s_bl.Engineer.ReadAllDelete();
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
            int id = int.Parse(Console.ReadLine()!);
            BO.Engineer engineer = s_bl.Engineer.Read(id)!;

            string? name = engineer.Name;
            if (change("Name"))
            {
                Console.WriteLine("Enter the name of the engineer (full name)");
                name = Console.ReadLine()!;
            }

            string? email = engineer.Email;
            if (change("Email"))
            {
                Console.WriteLine("Enter an email address");
                email = Console.ReadLine()!;
            }

            BO.Enums.EngineerExperience difficulty = engineer.Level;
            if (change("Level"))
            {
                Console.WriteLine("Enter the level of the engineer");
                difficulty = (BO.Enums.EngineerExperience)int.Parse(Console.ReadLine()!);
            }

            double? cost  = engineer.Cost;
            if (change("Cost"))
            {
                Console.WriteLine("Enter an hourly cost");
                cost = double.Parse(Console.ReadLine()!);
            }

            engineer = new() { Id = id, Name = name, Email = email, Level = difficulty, Cost = cost, Task = null };
            try
            {
                s_bl.Engineer.Update(engineer);
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
            int id = int.Parse(Console.ReadLine()!);
            try
            {
                s_bl.Engineer.Delete(id);
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
                Menue menue;
                menueM();
                string userInput = Console.ReadLine()!;
                while (Enum.TryParse(userInput, out menue) && menue != Menue.Exit)
                {
                    try
                    {
                        switch (menue)
                        {
                            case Menue.Task:
                                taskCase();
                                break;
                            case Menue.Engineer:
                                engineerCase();
                                break;
                            case Menue.Time:
                                timeCase();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    menueM();
                    userInput = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
