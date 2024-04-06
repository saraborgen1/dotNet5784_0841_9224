using BO;
using PL.Task;
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

            try
            {
                 EngineerProperty = s_bl.Engineer.Read(engineerId)!;
                if(EngineerProperty!.Task!=null) 
                    EngineerWorkerProperty=s_bl.Task.Read(EngineerProperty.Task.Id);
                TaskList = new ObservableCollection<BO.Task>(s_bl.Engineer.AvailableTasks(EngineerProperty));
                if (EngineerWorkerProperty.Dependencies != null)
                    DependenciesProperty = new ObservableCollection<BO.TaskInList>(EngineerWorkerProperty.Dependencies);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            InitializeComponent();
        }
        public ObservableCollection<BO.TaskInList> DependenciesProperty
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(DependenciesPropertyProperty); }
            set { SetValue(DependenciesPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DependenciesProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DependenciesPropertyProperty =
            DependencyProperty.Register("DependenciesProperty", typeof(ObservableCollection<BO.TaskInList>), typeof(EngineerWorker), new PropertyMetadata(null));

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

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.Update(EngineerWorkerProperty);
                MessageBox.Show("The Task was successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
