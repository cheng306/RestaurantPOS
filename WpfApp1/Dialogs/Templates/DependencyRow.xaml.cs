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

namespace RestaurantPOS.Dialogs.Templates
{
    // logic here only apply to apperance of the dependencyRow
    public partial class DependencyRow : UserControl
    {
        

        public DependencyRow()
        {
            InitializeComponent();
            inventoryComboBox.ItemsSource = ((MainWindow)Application.Current.MainWindow).inventoryPage.inventoryList;
        }

        public string InventoryName
        {
            get { return (String)inventoryComboBox.SelectedItem; }
        }

        public double ConsumptionQuantity
        {
            get { return Double.Parse(quantityTextBox.Text); }
        }

        public bool ValidInventoryComboBox
        {
            get { return inventoryComboBox.SelectedItem != null; }
        }

        public bool ValidQuantityTextBox
        {
            get { return Double.TryParse(quantityTextBox.Text, out double dump); }
        }

        public bool ValidDependencyRow
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

        private void InventoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inventoryComboBoxBorder.BorderBrush = new SolidColorBrush(Colors.Black);
        }
    }
}
