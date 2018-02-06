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

namespace WpfApp1.Windows
{
    /// <summary>
    /// Interaction logic for AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        public AddItemWindow()
        {
            InitializeComponent();
            OtherInitializeSetup();
        }

        private void OtherInitializeSetup()
        {
            addButton.IsEnabled = false;     
        }

        internal String ItemName
        {
            get { return nameTextBox.Text; }
        }

        internal String ItemCategory
        {
            get { return nameTextBox.Text; }
        }




    }
}
