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

        /// <summary>
        /// con
        /// </summary>
        public EngineerListWindow()
        {
            _engineers = s_bl?.Engineer.ReadAll()!.ToList()!;
            EngineerList = new ObservableCollection<BO.Engineer>(_engineers);
            InitializeComponent();
        }

        /// <summary>
        /// EngineerList Property
        /// </summary>
        public ObservableCollection<BO.Engineer> EngineerList
        {
            get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }


        /// <summary>
        /// makes EngineerList  DependencyProperty
        /// </summary>
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        /// <summary>
        /// Level Property
        /// </summary>
        public BO.Enums.EngineerExperience Level { get; set; } = BO.Enums.EngineerExperience.None;
        List<BO.Engineer> _engineers;

        /// <summary>
        /// Respond to the value change event and cause the list of items to be updated on the display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_EngineerLevelFilter(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = new ObservableCollection<BO.Engineer>((Level == BO.Enums.EngineerExperience.None) ?
            _engineers : _engineers.Where(item => item.Level == Level));

        }

        /// <summary>
        /// Adds or deletes an entity
        /// </summary>
        /// <param name="item"> id of entity and if we are adding or updating</param>
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
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// opens EngineerWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_EngineerWindow(object sender, RoutedEventArgs e)
        {
            new EngineerWindow(addOrUpdateChanged).ShowDialog();
        }

        /// <summary>
        /// Opening a single item display window in update mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedEngineer(object sender, SelectionChangedEventArgs e)
        {
            BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
            if (engineer != null)
            {
                new EngineerWindow(addOrUpdateChanged, engineer.Id).ShowDialog();
            }

        }
        private void TextBox_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string searchText = textBox.Text.ToLower();
            EngineerList = new ObservableCollection<BO.Engineer>(_engineers.Where(engineer => engineer.Name.ToLower().Contains(searchText)));
        }

    }
}