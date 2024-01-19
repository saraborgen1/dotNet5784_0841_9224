namespace Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextTaskId { get => GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); } 
    internal static int NextDependencyId { get => GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); } 
}