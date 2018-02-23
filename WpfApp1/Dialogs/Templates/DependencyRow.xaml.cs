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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.Dialogs.Templates
{
    /// <summary>
    /// Interaction logic for DependencyRow.xaml
    /// </summary>
    public partial class DependencyRow : UserControl
    {
        

        public DependencyRow()
        {
            InitializeComponent();
            inventoryComboBox.ItemsSource = ((MainWindow)Application.Current.MainWindow).inventoryPage.inventoryList;
        }

        public bool ValidInventoryComboBox
        {
            get { return inventoryComboBox.SelectedItem != null; }
        }

        public bool ValidQuantityTextBox
        {
            get { return Double.TryParse(quantityTextBox.Text, out double dump); }
        }

        public bool Valid
        {
            get { return this.ValidQuantityTextBox && this.ValidInventoryComboBox; }
        }

        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this.ValidQuantityTextBox)
            {
                quantityTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                quantityTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
            }
        }
        ///

    }
}
