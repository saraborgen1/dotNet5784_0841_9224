using System;
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

namespace PL.Admin
{
    /// <summary>
    /// Interaction logic for WindowGantt.xaml
    /// </summary>
    public partial class WindowGantt : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public WindowGantt()
        {
            TaskListProperty = s_bl.Task.ReadAll();
            InitializeComponent();

        }



        public IEnumerable<BO.Task> TaskListProperty
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListPropertyProperty); }
            set { SetValue(TaskListPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskListProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListPropertyProperty =
            DependencyProperty.Register("TaskListProperty", typeof(IEnumerable<BO.Task>), typeof(WindowGantt), new PropertyMetadata(0));


    }
}
