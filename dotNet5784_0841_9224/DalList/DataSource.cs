namespace Dal;

internal static class DataSource
{
    internal static List<DO.Task> Tasks { get; } = new();

    internal static class Config
    {
        internal const int startTaskId = 0;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }
    }
}
