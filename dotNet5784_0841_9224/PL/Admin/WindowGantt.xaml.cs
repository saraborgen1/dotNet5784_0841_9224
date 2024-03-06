using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
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
            DataGrid? dataGrid = sender as DataGrid; //the graphic container

            DataTable dataTable = new DataTable(); //the logic container

            //add COLUMNS to datagrid and datatable
            if (dataGrid != null)
            {
                dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Task Id", Binding = new Binding("[0]") });
                dataTable.Columns.Add("Task Id", typeof(int));

                dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Task Name", Binding = new Binding("[1]") });
                dataTable.Columns.Add("Task Name", typeof(string));

                dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Engineer Id", Binding = new Binding("[2]") });
                dataTable.Columns.Add("Engineer Id", typeof(int));

                dataGrid.Columns.Add(new DataGridTextColumn() { Header = "Engineer Name", Binding = new Binding("[3]") });
                dataTable.Columns.Add("Engineer Name", typeof(string));

                int col = 4;
                for (DateTime day = (DateTime)s_bl.State.StartProject!; day <= (DateTime)s_bl.State.EndProject!; day = day.AddDays(1))
                {
                    string strDay = $"{day.Day}/{day.Month}/{day.Year}"; //"21/2/2024"
                    dataGrid.Columns.Add(new DataGridTextColumn() { Header = strDay, Binding = new Binding($"[{col}]") });
                    dataTable.Columns.Add(strDay, typeof(bool));
                    col++;
                }
            }

            //add ROWS to logic container (data table)
            //IEnumerable<BO.Task> orderedlistTasksScheduale = TaskListProperty.OrderBy(t => t.StartDate);
            foreach (BO.Task task in TaskListProperty/*orderedlistTasksScheduale*/)
            {
                //dataGrid.CellStyle

                DataRow row = dataTable.NewRow();
                row[0] = task.Id;
                row[1] = task.Alias;
                row[2] = task.Engineer.Id;
                row[3] = task.Engineer.Name;

                for (DateTime day = (DateTime)s_bl.State.StartProject!; day <= (DateTime)s_bl.State.EndProject!; day = day.AddDays(1))
                {
                    string strDay = $"{day.Day}/{day.Month}/{day.Year}"; //"21/2/2024"

                    if (day < task.StartDate || day > task.ForecastDate)

                        row[strDay] = false;
                    else
                    {

                        row[strDay] = true;
                    }
                }
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
