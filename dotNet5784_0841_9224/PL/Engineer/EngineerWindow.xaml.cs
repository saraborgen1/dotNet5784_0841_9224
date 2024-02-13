using System;
using System.Collections.Generic;
using System.Linq;
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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerWindow(int id =0)
        {
            BO.Engineer engineer;
            InitializeComponent();
            if (id == 0)
            {
                engineer = new BO.Engineer();
            }
            else
                try
                {
                    engineer = s_bl.Engineer.Read(id)!;
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


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
