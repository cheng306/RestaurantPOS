﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : UserControl
    {
        public TestPage()
        {
            InitializeComponent();
            ObservableCollection<string> oc = new ObservableCollection<string>();
            oc.Add("abc");
            oc.Add("abc");
            oc.Add("abc");

            lb1.ItemsSource = oc;
            lb2.ItemsSource = oc;
        }

       
        private void uc_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("TestPage loaded");                             
        }

        private void uc_Unloaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("TestPage unloaded");
        }

        private void uc_Initialized(object sender, EventArgs e)
        {
            Console.WriteLine("TestPage initialized");
        }
    }
}
