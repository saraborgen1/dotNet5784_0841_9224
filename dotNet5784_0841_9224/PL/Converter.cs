using BO;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PL;

/// <summary>
/// converts num into bool for add or  update
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

class ConvertSchedulingIsEnabled : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return s_bl.State.StartProject == null ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertIdEngineerIsEnabled : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isEnabled = value != null && (bool)value;
        return isEnabled ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class CellColorConverter : IMultiValueConverter
{
    Color[] arrColors = { Colors.White, Colors.Red, Colors.Green, Colors.RoyalBlue, Colors.Orange, Colors.LightYellow };
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (values[0] is DataGridCell cell && values[1] is DataRowView rowView)
        {
            try
            {
                DataRow row = rowView.Row;
                string columnName = (string)cell.Column.Header;
                int columnIndex = cell.Column.DisplayIndex;

                if (columnIndex < 2)
                {
                    return new SolidColorBrush(Colors.LightGoldenrodYellow);
                }

                DateTime currentDate = s_bl.State.CurrentDate;
                DateTime day;
                if (DateTime.TryParse(columnName, out day))
                {
                    int? taskId = row.Field<int>("Task Id");
                    if (taskId.HasValue)
                    {
                        var task = s_bl.Task.Read(taskId.Value);
                        if (day < task.ScheduledDate || day > task.ForecastDate)
                        {
                            cell.Foreground = new SolidColorBrush(arrColors[0]);
                            return new SolidColorBrush(arrColors[0]);
                        }
                        else
                        {
                            //המשימה הסתיימה
                            if (task.CompleteDate != null)
                            {
                                cell.Foreground = new SolidColorBrush(arrColors[2]);
                                return new SolidColorBrush(arrColors[2]);
                            }
                            // המסימה לא הושלמה והיא באיחור
                            if (currentDate > task.ForecastDate)
                            {
                                cell.Foreground = new SolidColorBrush(arrColors[1]);
                                return new SolidColorBrush(arrColors[1]);
                            }
                            //עוד לא התחילו את המשימה
                            if (task.StartDate == null)
                            {
                                //לא התחילו כי לא הגיע זמן ההתחלה 
                                if (task.ScheduledDate > currentDate)
                                {
                                    cell.Foreground = new SolidColorBrush(arrColors[3]);
                                    return new SolidColorBrush(arrColors[3]);
                                }
                                //לא התחילו את המשימה ועבר המועד המתוכנן להתחלה 
                                if (currentDate > task.ScheduledDate)
                                {
                                    cell.Foreground = new SolidColorBrush(arrColors[4]);
                                    return new SolidColorBrush(arrColors[4]);
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {
                return new SolidColorBrush(Colors.Black);
            }

        }
        return new SolidColorBrush(Colors.Silver);
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

public class NullToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class ConvertStatusToSetDates : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (s_bl.State.StatusProject() == BO.Enums.ProjectStatus.Creation);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class ConvertStatusToAuto : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (s_bl.State.StatusProject() == BO.Enums.ProjectStatus.Scheduling);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}

public class DependenciesConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is List<TaskInList> dependencies)
        {
            return string.Join(", ", dependencies.Select(dep => dep.ToString()));
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}