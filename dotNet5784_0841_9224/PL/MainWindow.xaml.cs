using PL.Engineer;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        /// <summary>
        /// CurrentDate Property Property
        /// </summary>
        public DateTime CurrentDateProperty
        {
            get { return (DateTime)GetValue(CurrentDatePropertyProperty); }
            set { SetValue(CurrentDatePropertyProperty, value); }
        }

        /// <summary>
        /// makes EngineerProperty DependencyProperty
        /// </summary>
        public static readonly DependencyProperty CurrentDatePropertyProperty =
          DependencyProperty.Register("CurrentDateProperty", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            CurrentDateProperty = s_bl.State.CurrentDate;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AdminWindow().Show();
        }

        private void Button_AddYear(object sender, RoutedEventArgs e)
        {
            s_bl.State.AddYear();
            CurrentDateProperty = s_bl.State.CurrentDate;
        }

        private void Button_AddMonth(object sender, RoutedEventArgs e)
        {
            s_bl.State.AddMonth();
            CurrentDateProperty = s_bl.State.CurrentDate;
        }

        private void Button_AddDay(object sender, RoutedEventArgs e)
        {
            s_bl.State.AddDay();
            CurrentDateProperty = s_bl.State.CurrentDate;

        }

        private void Button_AddWeek(object sender, RoutedEventArgs e)
        {
            s_bl.State.AddWeek();
            CurrentDateProperty = s_bl.State.CurrentDate;

        }

        private void Button_Enter(object sender, RoutedEventArgs e)
        {
           // if()
                new EngineerWorker().Show();
        }
    }
}