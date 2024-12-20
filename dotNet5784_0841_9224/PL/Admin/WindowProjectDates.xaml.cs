﻿using PL.Engineer;
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
            //MinumunTimeProperty = s_bl.State.MinimumDays();
            InitializeComponent();
        }

        private void Button_SetDates(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.State.SetProjectDates(StartDateProperty, (DateTime)EndDateProperty!);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Dates were updated successfuly", "", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }



        public DateTime StartDateProperty
        {
            get { return (DateTime)GetValue(StartDatePropertyProperty); }
            set { SetValue(StartDatePropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartDatePropertyProperty =
            DependencyProperty.Register("StartDateProperty", typeof(DateTime), typeof(WindowProjectDates), new PropertyMetadata(null));
        public DateTime? EndDateProperty
        {
            get { return (DateTime?)GetValue(EndDatePropertyProperty); }
            set { SetValue(EndDatePropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndDatePropertyProperty =
            DependencyProperty.Register("EndDateProperty", typeof(DateTime?), typeof(WindowProjectDates), new PropertyMetadata(null));

        private void SelectStartDate(object sender, SelectionChangedEventArgs e)
        {
            // Cast sender to DatePicker
            var datePicker = sender as DatePicker;

            // Check if the cast was successful and the SelectedDate has value
            if (datePicker != null && datePicker.SelectedDate.HasValue)
            {
                // Extract the date value if it exists
                StartDateProperty = datePicker.SelectedDate.Value;
                LimitEndProperty = datePicker.SelectedDate.Value + s_bl.State.MinimumDays();
            }
        }

        private void SelectEndDate(object sender, SelectionChangedEventArgs e)
        {
            // Cast sender to DatePicker
            var datePicker = sender as DatePicker;

            // Check if the cast was successful and the SelectedDate has value
            if (datePicker != null && datePicker.SelectedDate.HasValue)
            {
                // Extract the date value if it exists
                EndDateProperty = datePicker.SelectedDate.Value;
                LimitStartProperty = datePicker.SelectedDate.Value - s_bl.State.MinimumDays();
            }
        }
        public DateTime LimitStartProperty
        {
            get { return (DateTime)GetValue(LimitStartPropertyProperty); }
            set { SetValue(LimitStartPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LimitStartProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LimitStartPropertyProperty =
            DependencyProperty.Register("LimitStartProperty", typeof(DateTime), typeof(WindowProjectDates), new PropertyMetadata(null));


        public DateTime LimitEndProperty
        {
            get { return (DateTime)GetValue(LimitEndPropertyProperty); }
            set { SetValue(LimitEndPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LimitEndProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LimitEndPropertyProperty =
            DependencyProperty.Register("LimitEndProperty", typeof(DateTime), typeof(WindowProjectDates), new PropertyMetadata(null));



    }
}
