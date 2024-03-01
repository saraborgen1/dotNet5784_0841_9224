using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Admin
{
    /// <summary>
    /// Interaction logic for WindowProjectDates.xaml
    /// </summary>
    public partial class WindowProjectDates : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public WindowProjectDates()
        {
            InitializeComponent();
        }

        private void Button_SetDates(object sender, RoutedEventArgs e)
        {

        }



        public BlApi.IState StateProperty
        {
            get { return (BlApi.IState)GetValue(StatePropertyProperty); }
            set { SetValue(StatePropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatePropertyProperty =
            DependencyProperty.Register("StateProperty", typeof(BlApi.IState), typeof(WindowProjectDates), new PropertyMetadata(0));


    }
}
