namespace Dal;

internal static class DataSource
{    internal static class Config
    {
        /// <summary>
        /// A running ID number for the tasks
        /// </summary>
        internal const int startTaskId = 1;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }
        /// <summary>
        /// A running ID number for the dependency
        /// </summary>
        internal const int startDependencyId = 1;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }

        /// <summary>
        /// An propotie that represents the project start date
        /// </summary>
        public static DateTime? startProject=null;
        /// <summary>
        /// An propotie that represents the project end date
        /// </summary
        public static DateTime? endProject=null;

        public static DateTime currentDate = DateTime.Now;
        public static string adminPassword = "12345678";
        public static int adminUserId = 100000000;
    }
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.Dependency> Dependencys { get; } = new();
}
