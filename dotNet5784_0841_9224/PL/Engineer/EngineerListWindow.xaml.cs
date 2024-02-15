using System.Collections.ObjectModel;
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
            _engineers = s_bl?.Engineer.ReadAll()!.ToList()!;
            EngineerList = new ObservableCollection<BO.Engineer>(_engineers);
            InitializeComponent();
        }

        public ObservableCollection<BO.Engineer> EngineerList
        {
            get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));


        public BO.Enums.EngineerExperience Level { get; set; } = BO.Enums.EngineerExperience.None;
        List<BO.Engineer> _engineers;
        private void ComboBox_EngineerLevelFilter(object sender, SelectionChangedEventArgs e)
        {

            EngineerList = new ObservableCollection<BO.Engineer>((Level == BO.Enums.EngineerExperience.None) ?
            _engineers : _engineers.Where(item => item.Level == Level));

        }

        private void addOrUpdateChanged((int engineerId, bool isUpdateOrAdd) item)
        {
            try
            {
                var engineer = s_bl.Engineer.Read(item.engineerId);
                if (item.isUpdateOrAdd)
                {
                    EngineerList.Add(engineer!);
                    _engineers.Add(engineer!);
                }
                else
                {
                    var index = _engineers.FindIndex(e => e.Id == item.engineerId);
                    if (index is not -1)
                    {
                        EngineerList[index] = engineer!;
                        _engineers[index] = engineer!;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_EngineerWindow(object sender, RoutedEventArgs e)
        {
            new EngineerWindow(addOrUpdateChanged).ShowDialog();
        }

        private void SelectedEngineer(object sender, RoutedEventArgs e)
        {
            BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
            new EngineerWindow(addOrUpdateChanged, engineer!.Id).ShowDialog();

        }
    }
}
