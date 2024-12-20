﻿using PL.Admin;
using PL.Engineer;
using PL.Task;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public AdminWindow()
        {
            InitializeComponent();
        }
        private void btnEngineers_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        private void btnInitialization_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Do you want to initialize the data?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbResult == MessageBoxResult.Yes)
            {
                DalTest.Initialization.Reset();
                DalTest.Initialization.Do();
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Do you want to reset the data?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbResult == MessageBoxResult.Yes)
                DalTest.Initialization.Reset();
        }

        private void btnGantt_Click(object sender, RoutedEventArgs e)
        {
            if(s_bl.State.StatusProject()!= BO.Enums.ProjectStatus.Start)
            {
                MessageBox.Show("No start dates for any tasks", "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            new WindowGantt().Show();
        }


        private void btnTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();
        }

        private void btnAutoScheduling_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Task.AutoScheduling();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("The auto scheduling was done successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnProjectDates_Click(object sender, RoutedEventArgs e)
        {
            if(s_bl.State.StatusProject() != BO.Enums.ProjectStatus.Creation)
            {
                MessageBox.Show("The project start and end date cannot be reset after the entire project has been given dates", "Exeption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            new WindowProjectDates().Show();
        }

        private void DeletedEngineerWindow_Click(object sender, RoutedEventArgs e)
        {
            new DeletedEngineerWindow().Show();
        }
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
