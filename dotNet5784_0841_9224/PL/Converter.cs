﻿using System.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using static BO.Enums;

namespace PL;

/// <summary>
/// converts num into bool for add or update
/// </summary>
class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// converts num into bool for IsEnabled
/// </summary>
class ConvertIdIsEnabled : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


//class ImageConverter: IValueConverter
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//{ }
//    try
//    {
//    if(!File.Exists((string)Value))
//    throw new Exception(" ");
//BitmapImage b=new BitmapImage(new Uri((string)value,Urikind.Relati))


//public class CellColorConverter : IMultiValueConverter
//{
//    Color[] arrColors = (Colors.Purple, Colors.Pink, Colors.Plum, Colors.RoyalBlue, Colors.SeaGreen, Colors.LightYellow);
//    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//    {
//        if (values[0] is DataGridCell cell && values[1] is DataRow row)
//        {
//            try
//            {
//                string columnName = (string)cell.Column.Header;
//                //bool content =row.Field<bool>(columnName);
//                int columnIndex = cell.Column.DisplayIndex;

//                if (columnIndex < 4)
//                {
//                    //cell FontStyle = FontStyles.Italic;
//                    //cell FontWeight = FontWeight.Bold;
//                    //cell FontSize = 20;
//                    return new SolidColorBrush(Colors.LightGoldenrodYellow);
//                }

//                bool content = row.Field<bool>(columnName);
//                string? nameOfE = row.Field<string>("Engineer Name");
//                int? taskId = row.Field<int>("Task Id");

//                if (content == true)
//                {
//                    Color color = arrColors[taskId ?? 0];
//                    cell.Foreground = new SolidColorBrush(color);
//                    return new SolidColorBrush(color);
//                    //return GetColorFromName(nameOfE);//new SolidColorBrush(Colors.L Colors.LightGreen);
//                }
//                if (content == false)
//                {
//                    cell.Foreground = Brushes LightGray;
//                    return new SolidColorBrush(Colors LightGray);
//                }
//                //return new SolidColorBrush(Colors LightSteelBlue);
//            }
//            catch (Exception)
//            {
//                return new SolidColorBrush(Colors.Black);//Error! An Exception was thrown 
//            }
//        }
//        return new SolidColorBrush(Colors.DarkRed);// Error! object[] is invalid.
//    }
//    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//    {
//        throw new NotSupportedException();
//    }

//    Color GetColorFromName(string name)
//    {
//        byte r = (byte)(name[0] + 25);
//        byte g = (byte)(name[name.Length / 2] + 125);
//    }
//}
//}

public class StatusToColorBrushConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        string taskStatus = values[0] as string;
        bool isDependency = (bool)values[1];

        if (isDependency || taskStatus == "EMPTY")
            return Brushes.Red; // Task is delayed or it's a dependency
        else if (taskStatus == "FULL")
            return Brushes.Green; // Task is on schedule
        else
            return Brushes.Blue; // Task not started yet
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertStringToInt : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertSetDatesIsEnabled : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (/* value.StartDateProperty &&value.EndDateProperty &&*/ s_bl.State.StartProject != null && s_bl.State.EndProject != null) ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class TimeSpanToWidthConverter : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.Task task = (value as BO.Task);
        return (int)((TimeSpan)task.RequiredEffortTime!).TotalDays;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
