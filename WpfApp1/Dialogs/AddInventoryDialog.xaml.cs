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

namespace RestaurantPOS.Dialogs
{
    /// <summary>
    /// Interaction logic for AddInventoryWindow.xaml
    /// </summary>
    public partial class AddInventoryDialog : Window
    {
        bool validName;
        bool validQuantity;

        public AddInventoryDialog()
        {
            InitializeComponent();
            OtherInitialSetup();
        }

        private void OtherInitialSetup()
        {
            nameWarningTextBlock.Visibility = Visibility.Hidden;
            quantityWarningTextBlock.Visibility = Visibility.Hidden;
            addButton.IsEnabled = false;
        }

        internal string InventoryName
        {
            get { return nameTextBox.Text; }
        }

        internal double InventoryQuantity
        {
            get { return Double.Parse(quantityTextBox.Text); }
        }

        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Double.TryParse(quantityTextBox.Text, out double dump))
            {
                validQuantity = true;
                quantityWarningTextBlock.Visibility = Visibility.Hidden;
                UpdateAddButton();
            }
            else
            {
                validQuantity = false;
                quantityWarningTextBlock.Visibility = Visibility.Visible;
                UpdateAddButton();
            }
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!nameTextBox.Text.Equals(""))
            {
                validName = true;
                nameWarningTextBlock.Visibility = Visibility.Hidden;
                UpdateAddButton();
            }
            else
            {
                validName = false;
                nameWarningTextBlock.Visibility = Visibility.Visible;
                UpdateAddButton();
            }
        }

        private void UpdateAddButton()
        {
            if (validName && validQuantity)
            {
                addButton.IsEnabled = true;
            }
            else
            {
                addButton.IsEnabled = false;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
