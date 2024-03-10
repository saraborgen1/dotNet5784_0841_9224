using PL.Engineer;
using PL.Task;
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
                var _engineertask = s_bl.Task.ReadAll(s => s.Engineer!.Id== engineerId).ToList()!;
               EngineerWorkerProperty = _engineertask.FirstOrDefault(p => p.Engineer!.Id == engineerId)!;
                EngineerProperty = s_bl.Engineer.Read(engineerId)!;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

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


        private void Button_viewList(object sender, RoutedEventArgs e)
        {
            try
            {
                new TaskListWindow(EngineerProperty, false).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }
    }

}
