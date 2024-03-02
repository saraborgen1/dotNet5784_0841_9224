using BlApi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

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

class ConvertSetDatesIsEnabled : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return  (/* value.StartDateProperty &&value.EndDateProperty &&*/ s_bl.State.StartProject!=null&& s_bl.State.EndProject != null) ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

//class DateToWidthConverter : IValueConverter
//{
//    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        BO.Task task = (value as BO.Task);
//        return (int)((TimeSpan)task.RequiredEffortTime!).TotalDays;
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        throw new NotImplementedException();
//    }
//}

public class TaskStatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var taskItem = value as TaskIte;
        if (taskItem == null) return Brushes.Transparent;

        if (taskItem.IsDelayed(DateTime.Now))
            return Brushes.Red; // מתעכבת
        else if (taskItem.IsCompleted)
            return Brushes.Green; // הושלמה
        else
            return Brushes.Blue; // לא התחילה / בתהליך
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
