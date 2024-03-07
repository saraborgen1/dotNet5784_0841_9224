using PL.Engineer;
using System;
using System.Collections;
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //DepList = new ObservableCollection<BO.TaskInList?>(s_bl.Task.Read(id)!.Dependencies);
            int i = 1;
        }
        public ObservableCollection<BO.TaskInList?> DepList
        {
            get { return (ObservableCollection<BO.TaskInList?>)GetValue(DepListProperty); }
            set { SetValue(DepListProperty, value); }
        }
        public static readonly DependencyProperty DepListProperty =
           DependencyProperty.Register("DepList", typeof(IEnumerable<BO.TaskInList?>), typeof(EngineerListWindow), new PropertyMetadata(null));


    }
}
