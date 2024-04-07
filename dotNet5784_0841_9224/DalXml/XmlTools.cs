namespace Dal;
using DO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

public static class XMLTools
{
    #region xmlConvertor
    /// <summary>
    /// Converts item to xelement
    /// </summary>
    /// <typeparam name="Item">the type item that will be converted</typeparam>
    /// <param name="item">the item that will be converted<</param>
    /// <param name="name">The name of the item type</param>
    /// <returns>xelement of the item</returns>
    internal static XElement itemToXelement<Item>(Item item, string name) where Item : new()
    {
        IEnumerable<PropertyInfo> items = item!.GetType().GetProperties();

        IEnumerable<XElement> xElements = from propInfo in items
                                          select new XElement(propInfo.Name, propInfo.GetValue(item)!.ToString());
       
        return new XElement(name, xElements);
    }

    /// <summary>
    /// Converts xelement to item
    /// </summary>
    /// <typeparam name="Item">The type of the item</typeparam>
    /// <param name="xElement">the xelement that will be converted </param>
    /// <returns>converted xelement in item type</returns>
    internal static Item xelementToItem<Item>(XElement xElement) where Item : new()
    {
        Item newItem = new Item();

        IEnumerable<XElement> elements = xElement.Elements().ToList();
        Dictionary<string, PropertyInfo> items = newItem.GetType().GetProperties().ToDictionary(k => k.Name, v => v);
        foreach (var element in elements)
        { 
            if (items.TryGetValue(element.Name.LocalName, out var propertyInfo))
            {
                var propertyType = propertyInfo.PropertyType;
                var underlyingType = Nullable.GetUnderlyingType(propertyType);

                propertyInfo.SetValue(newItem,Convert.ChangeType(element.Value, underlyingType ?? propertyType));
            }
        }
        return newItem;
    }

    /// <summary>
    /// converts  xelemnt to a collection of items
    /// </summary>
    /// <typeparam name="Item">the generic type</typeparam>
    /// <param name="xElement">the xelement that will be converted</param>
    /// <returns>a collection of the converted elements of the xelement in item type</returns>
    internal static IEnumerable<Item> xelementToItems<Item>(XElement xElement) where Item : new()
    =>    from element in xElement.Elements()
               select xelementToItem<Item>(element);
    #endregion


    static IEnumerable<string> GetEnumDescriptions<TEnum>() where TEnum : struct, Enum
    {
        var enumType = typeof(TEnum);

        IEnumerable<TEnum> enumValues = Enum.GetValues(enumType).Cast<TEnum>();

        IEnumerable<string> descriptions = from enumValue in enumValues
                                           let fieldInfo = enumType.GetField(enumValue.ToString())
                                           let attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute
                                           select attribute?.Description ?? enumValue.ToString();

        return descriptions;
    }
    const string s_xml_dir = @"..\xml\";
    static XMLTools()
    {
        if (!Directory.Exists(s_xml_dir))
            Directory.CreateDirectory(s_xml_dir);
    }

    #region Extension Fuctions
    public static T? ToEnumNullable<T>(this XElement element, string name) where T : struct, Enum =>
        Enum.TryParse<T>((string?)element.Element(name), out var result) ? (T?)result : null;

    public static DateTime? ToDateTimeNullable(this XElement element, string name) =>
        DateTime.TryParse((string?)element.Element(name), out var result) ? (DateTime?)result : null;

    public static double? ToDoubleNullable(this XElement element, string name) =>
        double.TryParse((string?)element.Element(name), out var result) ? (double?)result : null;

    public static int? ToIntNullable(this XElement element, string name) =>
        int.TryParse((string?)element.Element(name), out var result) ? (int?)result : null;

    #endregion

    #region XmlConfig
    public static int GetAndIncreaseNextId(string data_config_xml, string elemName)
    {
        XElement root = LoadListFromXMLElement(data_config_xml);
        int nextId = root.ToIntNullable(elemName) ?? throw new FormatException($"can't convert id.  {data_config_xml}, {elemName}");
        root.Element(elemName)?.SetValue((nextId + 1).ToString());
        SaveListToXMLElement(root, data_config_xml);
        return nextId;
    }

    #endregion

    #region SaveLoadWithXElement
    public static void SaveListToXMLElement(XElement rootElem, string entity)
    {
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            rootElem.Save(filePath);
        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to create xml file: {s_xml_dir + filePath}, {ex.Message}");
        }
    }

    public static XElement LoadListFromXMLElement(string entity)
    {
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            if (File.Exists(filePath))
                return XElement.Load(filePath);
            XElement rootElem = new(entity);
            rootElem.Save(filePath);
            return rootElem;
        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to load xml file: {s_xml_dir + filePath}, {ex.Message}");
        }
    }
    #endregion

    #region SaveLoadWithXMLSerializer
    //public static void SaveListToXMLSerializer<T>(List<T?> list, string entity) where T : struct
    public static void SaveListToXMLSerializer<T>(List<T> list, string entity) where T : class
    {
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            using FileStream file = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            new XmlSerializer(typeof(List<T>)).Serialize(file, list);
            //new XmlSerializer(typeof(List<T?>)).Serialize(file, list);

        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to create xml file: {s_xml_dir + filePath}, {ex.Message}");
        }
    }

    //public static List<T?> LoadListFromXMLSerializer<T>(string entity) where T : struct
    public static List<T> LoadListFromXMLSerializer<T>(string entity) where T : class
    {
        string filePath = $"{s_xml_dir + entity}.xml";
        try
        {
            if (!File.Exists(filePath)) return new();
            using FileStream file = new(filePath, FileMode.Open);
            XmlSerializer x = new(typeof(List<T>));
            return x.Deserialize(file) as List<T> ?? new();
            //XmlSerializer x = new(typeof(List<T?>));
            //return x.Deserialize(file) as List<T?> ?? new();

        }
        catch (Exception ex)
        {
            throw new DalXMLFileLoadCreateException($"fail to load xml file: {filePath}, {ex.Message}");
        }
    }
    #endregion

}