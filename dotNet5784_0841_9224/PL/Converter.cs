using BO;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
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

class ConvertIdToDeleteVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? Visibility.Hidden : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class ConvertTaskToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? Visibility.Hidden : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
class ConvertIdToPasswordVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value != 0 ? Visibility.Hidden : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class CellColorConverter : IMultiValueConverter
{
    Color[] arrColors = { Colors.White, Colors.Red, Colors.Green, Colors.RoyalBlue, Colors.Orange, Colors.LightYellow ,Colors.Purple};
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
                        //if the task is done
                        if(task.Status== Enums.Status.Done) 
                        {
                            if(day>=task.StartDate && day<=task.CompleteDate)//color the time spent on it -> green
                            {
                                cell.Foreground = new SolidColorBrush(arrColors[2]);
                                return new SolidColorBrush(arrColors[2]);
                            }
                        }
                        //if the mission was started but not finished
                        if (task.Status == Enums.Status.OnTrack)
                        {
                            if (currentDate > task.ForecastDate)//if the mission is late
                            {
                                if (day >= task.StartDate && day < currentDate)//red
                                {
                                    cell.Foreground = new SolidColorBrush(arrColors[1]);
                                    return new SolidColorBrush(arrColors[1]);
                                }
                            }
                            //if it was started but is not late , this is the progress
                            if(day<currentDate && day>= task.StartDate)//purple
                            {
                                cell.Foreground = new SolidColorBrush(arrColors[6]);
                                return new SolidColorBrush(arrColors[6]);
                            }
                            //how much time is left
                            if(day>=currentDate && day<= task.ForecastDate) //blue
                            {
                                cell.Foreground = new SolidColorBrush(arrColors[3]);
                                return new SolidColorBrush(arrColors[3]);
                            }
                        }
                        //if the task hasnt started
                        if (task.Status == Enums.Status.Scheduled)
                        {
                            if (currentDate > task.ScheduledDate)//late to start
                            {
                                if (day < currentDate && day >= task.ScheduledDate)//orange -> how late 
                                {
                                    cell.Foreground = new SolidColorBrush(arrColors[4]);
                                    return new SolidColorBrush(arrColors[4]);
                                }
                                if(day>=currentDate && day<= task.ForecastDate)//blue-> how much time left
                                {
                                    cell.Foreground = new SolidColorBrush(arrColors[3]);
                                    return new SolidColorBrush(arrColors[3]);
                                }
                            }
                            //not late, how much time to do 
                            if(day>=task.ScheduledDate && day<= task.ForecastDate) //blue
                            {
                                cell.Foreground = new SolidColorBrush(arrColors[3]);
                                return new SolidColorBrush(arrColors[3]);
                            }
                        }
                        cell.Foreground = new SolidColorBrush(arrColors[0]);
                        return new SolidColorBrush(arrColors[0]);

                    }
                }
            }
            catch (Exception)
            {
                cell.Foreground = new SolidColorBrush(arrColors[0]);
                return new SolidColorBrush(arrColors[0]);
            }

        }
        return new SolidColorBrush(arrColors[0]);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
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