using System.Windows;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        bool isCreate = false;
        private event Action<(int engineerId, bool isUpdateOrAdd)> _onUpdateOrAdd;

        /// <summary>
        /// con
        /// </summary>
        /// <param name="onUpdateOrAdd"></param>
        /// <param name="id"></param>
        public EngineerWindow(Action<(int, bool)> onUpdateOrAdd, int id = 0)
        {
            _onUpdateOrAdd = onUpdateOrAdd;
            InitializeComponent();
            if (id == 0)
            {
                EngineerProperty = new BO.Engineer();
                isCreate = true;
            }
            else
                try
                {
                    EngineerProperty = s_bl.Engineer.Read(id)!;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

        }

        /// <summary>
        /// EngineerProperty Property
        /// </summary>
        public BO.Engineer EngineerProperty
        {
            get { return (BO.Engineer)GetValue(EngineerPropertyProperty); }
            set { SetValue(EngineerPropertyProperty, value); }
        }

        /// <summary>
        /// makes EngineerProperty DependencyProperty
        /// </summary>
        public static readonly DependencyProperty EngineerPropertyProperty =
          DependencyProperty.Register("EngineerProperty", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        /// <summary>
        /// adds  or update s\entity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_UpdateOrAdd(object sender, RoutedEventArgs e)
        {
            
            if (EngineerProperty.Id < 100000000 || EngineerProperty.Id > 1000000000)
            {
                MessageBox.Show("The Id must contain exactly 9 digits", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (EngineerProperty.Name == " ")
            {
               MessageBox.Show("You must enter a name", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (EngineerProperty.Email == null)
            {
                MessageBox.Show("Email must be entered", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (EngineerProperty.Email != null)
            {
                if (!EngineerProperty.Email.Contains("@"))
                {
                    MessageBox.Show("@ must appear in email address", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                };
                if (!EngineerProperty.Email.Contains("."))
                {
                    MessageBox.Show("A dot must appear in the email address", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                };
            }
            if (EngineerProperty.Cost == null || EngineerProperty.Cost <= 0)
            {
                MessageBox.Show("Must enter a positive salary for the employee", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }



            if (isCreate == true)

                try
                {
                    s_bl.Engineer.Create(EngineerProperty);
                    _onUpdateOrAdd((EngineerProperty.Id, true));
                    string? _password=s_bl.Engineer.GetPassword(EngineerProperty.Id);
                    MessageBox.Show($"Your engineer password is:{_password}", "password", MessageBoxButton.OK, MessageBoxImage.Information);
                    MessageBox.Show("The engineer was successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            try
            {
                s_bl.Engineer.Update(EngineerProperty);
                _onUpdateOrAdd((EngineerProperty.Id, false));
                MessageBox.Show("The engineer was successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Engineer.Delete(EngineerProperty.Id);
        }

 
    }
}
