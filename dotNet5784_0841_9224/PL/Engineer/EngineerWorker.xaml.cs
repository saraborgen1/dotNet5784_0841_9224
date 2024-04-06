using BO;
using PL.Task;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
                {
                    EngineerWorkerProperty = s_bl.Task.Read(EngineerProperty.Task.Id);

                    if (EngineerWorkerProperty.Dependencies != null)
                        DependenciesProperty = new ObservableCollection<BO.TaskInList>(EngineerWorkerProperty.Dependencies);
                }
             
                TaskList = new ObservableCollection<BO.Task>(s_bl.Engineer.AvailableTasks(EngineerProperty));
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
            if (s_bl.State.StatusProject() != Enums.ProjectStatus.Start)
            {
                MessageBox.Show("Engineers cannot be assigned tasks at this stage", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (EngineerProperty.Task != null)
            {
                MessageBox.Show("A new mission cannot be taken before the current mission is finished", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BO.Task? task = (sender as ListView)?.SelectedItem as BO.Task;
            if (task != null)
            {
                try
                {
                    var temp = new TaskInEngineer { Id = task.Id, Alias = task.Alias };
                    EngineerProperty.Task = temp;
                    s_bl.Engineer.Update(EngineerProperty);
                    var tempTask=s_bl.Task.Read(EngineerProperty.Task.Id);
                    tempTask.StartDate = s_bl.State.CurrentDate;
                    s_bl.Task.Update(tempTask);
                    this.Close();
                    MessageBox.Show("Task taken successfuly ", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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

        private void ButtonDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tempTask = s_bl.Task.Read(EngineerProperty.Task!.Id);
                tempTask.CompleteDate = s_bl.State.CurrentDate;
                s_bl.Task.Update(tempTask);
                this.Close();
                MessageBox.Show("Task finished successfuly ", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
