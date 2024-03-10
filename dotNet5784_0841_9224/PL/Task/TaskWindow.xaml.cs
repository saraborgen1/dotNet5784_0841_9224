using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PL.Task
{

    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        bool isCreate = false;
        private event Action<(int TaskId, bool isUpdateOrAdd)> _onUpdateOrAdd;


        public TaskWindow(Action<(int, bool)> onUpdateOrAdd, int id = 0)
        {
            _onUpdateOrAdd = onUpdateOrAdd;
            InitializeComponent();
            if (id == 0)
            {
                TaskProperty = new BO.Task();
                isCreate = true;
            }
            else
                try
                {
                    TaskProperty = s_bl.Task.Read(id)!;
                    TaskInListProperty = (s_bl.Task.ReadAll(p => p.Id != id)
                        .Select(task => s_bl.Task.GetTaskInList(task.Id))
                        .ToList()).ToList();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            if (TaskProperty.Dependencies != null)
                DependenciesProperty = new ObservableCollection<BO.TaskInList>(TaskProperty.Dependencies);
        }

        public BO.Task TaskProperty
        {
            get { return (BO.Task)GetValue(TaskPropertyProperty); }
            set { SetValue(TaskPropertyProperty, value); }
        }


        public static readonly DependencyProperty TaskPropertyProperty =
          DependencyProperty.Register("TaskProperty", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        private void Button_UpdateOrAdd(object sender, RoutedEventArgs e)
        {
            //בדיקות תקינות ובדיקות מה מותר לעדכן מה ובאיזה שלב
            if (DependenciesProperty != null)
                TaskProperty.Dependencies = new List<BO.TaskInList>(DependenciesProperty.ToList());


            if (isCreate == true)

                try
                {

                    s_bl.Task.Create(TaskProperty);
                    _onUpdateOrAdd((TaskProperty.Id, true));
                    MessageBox.Show("The Task was successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            try
            {
                s_bl.Task.Update(TaskProperty);
                _onUpdateOrAdd((TaskProperty.Id, false));
                MessageBox.Show("The Task was successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.Delete(TaskProperty.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        public List<BO.TaskInList> TaskInListProperty
        {
            get { return (List<BO.TaskInList>)GetValue(TaskInListPropertyProperty); }
            set { SetValue(TaskInListPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskInListProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskInListPropertyProperty =
            DependencyProperty.Register("TaskInListProperty", typeof(List<BO.TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));



        private void ComboBoxAllTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox combo)
            {
                if (combo != null)
                {
                    if (DependenciesProperty.Where(item => item == (combo.SelectedItem as BO.TaskInList)!).FirstOrDefault() != null)
                    {
                        MessageBox.Show("Cannot add dependency because it already exists ", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    DependenciesProperty.Add((combo.SelectedItem as BO.TaskInList)!);
                }
            }
        }


        public ObservableCollection<BO.TaskInList> DependenciesProperty
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(DependenciesPropertyProperty); }
            set { SetValue(DependenciesPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DependenciesProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DependenciesPropertyProperty =
            DependencyProperty.Register("DependenciesProperty", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));

        private void ComboBox_SelectionChangedDeleteDep(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox combo)
            {
                if (combo != null)
                    DependenciesProperty.Remove((combo.SelectedItem as BO.TaskInList)!);
            }
        }
    }
}
