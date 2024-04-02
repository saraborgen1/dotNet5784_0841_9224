using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public MainWindow()
        {
            InitializeComponent();
        }
        public DateTime CurrentDateProperty
        {
            get { return (DateTime)GetValue(CurrentDatePropertyProperty); }
            set { SetValue(CurrentDatePropertyProperty, value); }
        }

        /// <summary>
        /// makes EngineerProperty DependencyProperty
        /// </summary>
        public static readonly DependencyProperty CurrentDatePropertyProperty =
          DependencyProperty.Register("CurrentDateProperty", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(s_bl.State.CurrentDate));




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
            if (IdProperty == s_bl.AdminUserId)
                if (PasswordProperty == s_bl.AdminPassword)
                    new AdminWindow().Show();
                else
                {
                    MessageBox.Show("Wrong password", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            else
            {
                try
                {
                    //var engineer = s_bl.Engineer.Read(IdProperty);
                        if (s_bl.Engineer.comparePassword(IdProperty,PasswordProperty))
                            new EngineerWorker(IdProperty).Show();
                        else
                        {
                            MessageBox.Show("Wrong password", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

        }


        public string PasswordProperty
        {
            get { return (string)GetValue(PasswordPropertyProperty); }
            set { SetValue(PasswordPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PasswordProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordPropertyProperty =
            DependencyProperty.Register("PasswordProperty", typeof(string), typeof(MainWindow), new PropertyMetadata(null));


        public int IdProperty
        {
            get { return (int)GetValue(IdPropertyProperty); }
            set { SetValue(IdPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IdProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdPropertyProperty =
            DependencyProperty.Register("IdProperty", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
                    PasswordProperty = passwordBox.Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AdminWindow().Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new EngineerWorker(123456789).Show();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (var c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true; // מונע הקלדה של תווים שאינם ספרות
                    return;
                }
            }
        }

    }
}