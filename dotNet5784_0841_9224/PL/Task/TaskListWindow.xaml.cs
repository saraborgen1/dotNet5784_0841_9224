using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public TaskListWindow(bool flag = true, BO.Engineer _engineer = null!)
        {
            if (flag)
                _tasks = s_bl?.Task.ReadAll()!.ToList()!;
            else
            {
              
                _tasks = s_bl?.Task.ReadAll(s => (s.Engineer!.Id == 0 && s.Copmlexity <= _engineer!.Level && AreDependentTasksCompleted(s)==true))!.ToList()!;

            }
            TaskList = new List<BO.Task>(_tasks);
          
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public List<BO.Task> TaskList
        {
            get { return (List<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(List<BO.Task>), typeof(TaskListWindow), new PropertyMetadata(null));

        public BO.Enums.EngineerExperience Copmlexity { get; set; } = BO.Enums.EngineerExperience.None;
        List<BO.Task> _tasks;

        private void ComboBox_TaskLevelFilter(object sender, SelectionChangedEventArgs e)
        {
            TaskList = new List<BO.Task>((Copmlexity == BO.Enums.EngineerExperience.None) ?
           _tasks : _tasks.Where(item => item.Copmlexity == Copmlexity));
        }

        private void addOrUpdateChanged((int TaskId, bool isUpdateOrAdd) item)
        {
            try
            {
                var task = s_bl.Task.Read(item.TaskId);
                if (item.isUpdateOrAdd)
                {
                    TaskList.Add(task!);
                    _tasks.Add(task!);
                }
                else
                {
                    var index = _tasks.FindIndex(e => e.Id == item.TaskId);
                    if (index is not -1)
                    {

                        TaskList[index] = task!;
                        _tasks[index] = task!;

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_TaskWindow(object sender, RoutedEventArgs e)
        {
            new TaskWindow(addOrUpdateChanged).ShowDialog();
        }

        private void SelectedTask(object sender, SelectionChangedEventArgs e)
        {
            BO.Task? task = (sender as ListView)?.SelectedItem as BO.Task;
            if (task != null)
            {
                new TaskWindow(addOrUpdateChanged, task.Id).ShowDialog();
            }

        }
        public bool AreDependentTasksCompleted(BO.Task task)
        {
            // אם אין למשימה תלויות, מחזירים TRUE
            if (task.Dependencies == null || task.Dependencies.Count == 0)
                return true;

            // לולאה על כל התלויות של המשימה
            foreach (var dependentTask in task.Dependencies)
            {
                // אם המשימה התלויה עדיין לא הסתיימה, מחזירים FALSE
                if (dependentTask.Status == BO.Enums.Status.Done)
                    return false;

                // בדיקה רקורסיבית של המשימות התלויות של המשימה התלויה
                AreDependentTasksCompleted(s_bl.Task.Read(dependentTask.Id));
            }

            // אם עברנו על כל התלויות וכולן הסתיימו, מחזירים TRUE
            return true;
        }
    }
   
}
