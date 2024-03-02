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
        public EngineerWorker(int engineerId= 654567898)
        {
            InitializeComponent();
            try
            {
                var _engineertask = s_bl.Task.ReadAll(s => s.Engineer!.Id== engineerId);
                var _engincsceer = _engineertask.FirstOrDefault(p => p.Engineer!.Id == engineerId);
            //    EngineerWorkerProperty = _engineertask!;
                var _engineer = s_bl.Engineer.Read(engineerId);
               var _tasks = s_bl?.Task.ReadAll(s => (s.Engineer == null && s.Copmlexity <= _engineer!.Level /*&&אין משימה שלא הסתיימה*/))!.ToList()!;
                TaskList = new ObservableCollection<BO.Task>(_tasks);
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
          DependencyProperty.Register("EngineerProperty", typeof(BO.Task), typeof(EngineerWorker), new PropertyMetadata(null));

        public ObservableCollection<BO.Task> TaskList
        {
            get { return (ObservableCollection<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.Task>), typeof(TaskListWindow), new PropertyMetadata(null));
    }

}
