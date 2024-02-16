using BO;
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
        public EngineerWindow(Action<(int,bool)> onUpdateOrAdd, int id = 0)
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
                catch (Exception ex) { MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error); }

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
            if(EngineerProperty.Id==0|| EngineerProperty.Id<100000000|| EngineerProperty.Id > 1000000000) { }
            if(EngineerProperty.Name==" ") { }
            if (EngineerProperty.Email != null)
            {
                if (!EngineerProperty.Email.Contains("@")) { };
                if (!EngineerProperty.Email.Contains(".")) { };
            }
            if (EngineerProperty.Cost == null || EngineerProperty.Cost <= 0) { }

                if (isCreate == true)

                try
                {
                    s_bl.Engineer.Create(EngineerProperty);
                    _onUpdateOrAdd((EngineerProperty.Id, true));
                    MessageBox.Show("The engineer was successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
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
            }
        }
    }
}
