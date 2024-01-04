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
        private static int nextDependencyId = startTaskId;
        internal static int NextDependencyId { get => nextDependencyId++; }
    }
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.Dependency> Dependencys { get; } = new();
}
