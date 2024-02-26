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
        public TaskListWindow()
        {
            _tasks = s_bl?.Task.ReadAll()!.ToList()!;
            TaskList = new ObservableCollection<BO.Task>(_tasks);
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<BO.Task> TaskList
        {
            get { return (ObservableCollection<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.Task>), typeof(TaskListWindow), new PropertyMetadata(null));

        public BO.Enums.EngineerExperience Copmlexity { get; set; } = BO.Enums.EngineerExperience.None;
        List<BO.Task> _tasks;

        private void ComboBox_TaskLevelFilter(object sender, SelectionChangedEventArgs e)
        {
            TaskList = new ObservableCollection<BO.Task>((Copmlexity == BO.Enums.EngineerExperience.None) ?
           _tasks : _tasks.Where(item => item.Copmlexity == Copmlexity));
        }

        private void SelectedTask(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }
}
