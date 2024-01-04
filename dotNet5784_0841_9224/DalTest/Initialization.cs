namespace DalTest;
using DalApi;
using DO;
public static class Initialization
{

    private static ITask? s_dalTask; //stage 1
    private static IDependency? s_dalDependency; //stage 1
    private static IEngineer? s_dalEngineer; //stage 1
    /// <summary>
    /// Generating random numbers while filling in the object values
    /// </summary>
    private static readonly Random s_rand = new();

    private static void createTasks()
    {
        string[] taskAlies =
        {
            "RequirementAnalysis","Design","Implementation","Testing","Deployment","CodeReview",
            "Documentation","MeetingWithStakeholders"
        };
        string[] TaskDescriptions =
         {
        "Analyzing project requirements",
        "Planning the project and graphical interface",
        "Writing code and building the interface",
        "Testing and performing experiments",
        "Launching the system",
        "Code review and corrections",
        "Creating technical documents and guides",
        "Meetings with other stakeholders"
         };
        for (int i = 0; i < taskAlies.Length; i++) 
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime = start.AddDays(s_rand.Next(range));
            Task newTask = new();

            s_dalTask!.Create(newTask);
        }

    }








    f

}
