using BO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PL
{
    /// <summary>
    /// Interaction logic for EngineerWorker.xaml
    /// </summary>
    public partial class EngineerWorker : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerWorker(int engineerId)
        {
            InitializeComponent();
            try
            {
                var _engineertask = s_bl.Task.ReadAll(s => s.Engineer!.Id == engineerId).ToList()!;
                EngineerWorkerProperty = _engineertask.FirstOrDefault(p => p.Engineer!.Id == engineerId)!;
                EngineerProperty = s_bl.Engineer.Read(engineerId)!;
                _tasks = s_bl?.Task.ReadAll(s => (s.Engineer!.Id == null && s.Copmlexity <= EngineerProperty!.Level && AreDependentTasksCompleted(s)))!.ToList()!;
                TaskList = new ObservableCollection<BO.Task>(_tasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.Task>), typeof(EngineerWorker), new PropertyMetadata(null));

        public BO.Enums.EngineerExperience Copmlexity { get; set; } = BO.Enums.EngineerExperience.None;
        List<BO.Task> _tasks;

        public BO.Task EngineerWorkerProperty
        {
            get { return (BO.Task)GetValue(EngineerWorkerPropertyProperty); }
            set { SetValue(EngineerWorkerPropertyProperty, value); }
        }

        public static readonly DependencyProperty EngineerWorkerPropertyProperty =
          DependencyProperty.Register("EngineerWorkerProperty", typeof(BO.Task), typeof(EngineerWorker), new PropertyMetadata(null));



        public BO.Engineer EngineerProperty
        {
            get { return (BO.Engineer)GetValue(EngineerPropertyProperty); }
            set { SetValue(EngineerPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EngineerProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerPropertyProperty =
            DependencyProperty.Register("EngineerProperty", typeof(BO.Engineer), typeof(EngineerWorker), new PropertyMetadata(null));


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
                    return true;

                // בדיקה רקורסיבית של המשימות התלויות של המשימה התלויה
                AreDependentTasksCompleted(s_bl.Task.Read(dependentTask.Id));
            }

            // אם עברנו על כל התלויות וכולן הסתיימו, מחזירים TRUE
            return false;
        }
        private void SelectedTask(object sender, SelectionChangedEventArgs e)
        {
            BO.Task? task = (sender as ListView)?.SelectedItem as BO.Task;
            if (task != null)
            {
                try
                {
                    var temp = new TaskInEngineer { Id = task.Id, Alias = task.Alias };
                    EngineerProperty.Task = temp;
                    s_bl.Engineer.Update(EngineerProperty);
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

        }
    }
}
