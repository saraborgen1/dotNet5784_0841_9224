namespace DalTest;
using DalApi;
using DO;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

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
            while (s_dalEngineer!.Read(_id) != null);
            int _numforenum = s_rand.Next(0, 5);
            EngineerExperience _level = (EngineerExperience)_numforenum;
            int _cost;
            _cost = s_rand.Next(70, 301);

            Engineer newEngineer = new(_id, EngineerNames[i], EngineerEmails[i], _level, _cost);
            s_dalEngineer!.Create(newEngineer);

        }
    }

}

private static void createDependencys()
{
    
}
