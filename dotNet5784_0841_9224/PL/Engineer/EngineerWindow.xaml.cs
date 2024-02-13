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
        public EngineerWindow(int id = 0)
        {
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
                catch (Exception ex) { Console.WriteLine(ex); }

        }
        public BO.Engineer EngineerProperty
        {
            get { return (BO.Engineer)GetValue(EngineerPropertyProperty); }
            set { SetValue(EngineerPropertyProperty, value); }
        }

        public static readonly DependencyProperty EngineerPropertyProperty =
          DependencyProperty.Register("EngineerProperty", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));


        private void Button_UpdateOrAdd(object sender, RoutedEventArgs e)
        {
            if (isCreate == true)

                try
                {
                    s_bl.Engineer.Create(EngineerProperty);
                    MessageBox.Show("The engineer was successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex) { Console.WriteLine(ex); }

            try
            {
                s_bl.Engineer.Update(EngineerProperty);
                MessageBox.Show("The engineer was successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
    }
}
