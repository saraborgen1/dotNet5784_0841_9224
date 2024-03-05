using PL.Engineer;
using System;
using System.Collections;
using System.Collections.Generic;
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



        public int DepProperty
        {
            get { return (int)GetValue(DepPropertyProperty); }
            set { SetValue(DepPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DepProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DepPropertyProperty =
            DependencyProperty.Register("DepProperty", typeof(int), typeof(TaskWindow), new PropertyMetadata(0));


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
    }
}
