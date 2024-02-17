using System.Reflection;

namespace BO;

static class Tools
{
    /// <summary>
    /// a function that prints all of the fields of the entity
    /// </summary>
    /// <typeparam name="T">template  entity type</typeparam>
    /// <param name="t"> </param>
    /// <returns> info of entity</returns>
    public static string ToStringProperty<T>(this T t)
    {
        
        string str = "";
        if (t != null)
        {
            foreach (PropertyInfo item in t.GetType().GetProperties())
                str += "\n" + item.Name +

                ": " + item.GetValue(t, null);
        }
        return str;
    }
}