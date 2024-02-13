using System.Windows;
using System.Windows.Controls;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.ReadAll()!;
        }

        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

      
        public BO.Enums.EngineerExperience Level { get; set; } = BO.Enums.EngineerExperience.None;

        private void ComboBox_EngineerLevelFilter(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Level == BO.Enums.EngineerExperience.None) ?
            s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == Level)!;

        }

        private void Button_EngineerWindow(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }

        private void SelectedEngineer(object sender, RoutedEventArgs e)
        {
                BO.Engineer? engineer = (sender as ListView)?.SelectedItems as BO.Engineer;
                new EngineerWindow(engineer!.Id).ShowDialog();

        }
    }
}
