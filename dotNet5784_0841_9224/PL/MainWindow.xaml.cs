using PL.Engineer;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AdminWindow().Show();
        }
    }
}