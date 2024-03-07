using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            TaskListProperty = s_bl.Task.ReadAll().ToList();
            InitializeComponent();
            DataContext = this;

        }


        private void dataGridSched_Initialized(object sender, EventArgs e)
        {
            TaskListProperty = s_bl.Task.ReadAll().ToList();
            DataGrid? dataGrid = sender as DataGrid;

            DataTable dataTable = new DataTable();

            if (dataGrid != null)
            {
                dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Task Id", Binding = new Binding("[0]") });
                dataTable.Columns.Add("Task Id", typeof(int));

                dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Task Name", Binding = new Binding("[1]") });
                dataTable.Columns.Add("Task Name", typeof(string));

                int col = 2;
                for (DateTime day = (DateTime)s_bl.State.StartProject!; day <= (DateTime)s_bl.State.EndProject!; day = day.AddDays(1))
                {
                    string strDay = $"{day.Day}/{day.Month}/{day.Year}";
                    dataGrid.Columns.Add(new DataGridTextColumn() { Header = strDay, Binding = new Binding($"[{col}]") });
                    col++;
                }
            }


            foreach (BO.Task task in TaskListProperty)
            {

                DataRow row = dataTable.NewRow();
                row[0] = task.Id;
                row[1] = task.Alias;
                dataTable.Rows.Add(row);
            }

            if (dataGrid != null)
            {
                dataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        public List<BO.Task> TaskListProperty
        {
            get { return (List<BO.Task>)GetValue(TaskListPropertyProperty); }
            set { SetValue(TaskListPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskListProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListPropertyProperty =
            DependencyProperty.Register("TaskListProperty", typeof(List<BO.Task>), typeof(WindowGantt), new PropertyMetadata(null));



    }
}
