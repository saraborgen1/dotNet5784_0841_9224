using PL.Engineer;
using PL.Task;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public AdminWindow()
        {
            InitializeComponent();
        }
        private void btnEngineers_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        private void btnInitialization_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Do you want to initialize the data?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbResult == MessageBoxResult.Yes)
                DalTest.Initialization.Do();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Do you want to reset the data?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbResult == MessageBoxResult.Yes)
                DalTest.Initialization.Reset();
        }

        private void btnGantt_Click(object sender, RoutedEventArgs e)
        {

        }


        private void btnTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();
        }

        private void btnAutoScheduling_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.AutoScheduling();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            MessageBox.Show("The auto scheduling was done successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
