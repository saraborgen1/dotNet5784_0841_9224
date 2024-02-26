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
        public DateTime? CurrentDateProperty
        {
            get { return (DateTime?)GetValue(CurrentDatePropertyProperty); }
            set { SetValue(CurrentDatePropertyProperty, value); }
        }

        /// <summary>
        /// makes EngineerProperty DependencyProperty
        /// </summary>
        public static readonly DependencyProperty CurrentDatePropertyProperty =
          DependencyProperty.Register("CurrentDateProperty", typeof(DateTime?), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            CurrentDateProperty = s_bl.State.CurrentDate;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AdminWindow().Show();
        }
    }
}