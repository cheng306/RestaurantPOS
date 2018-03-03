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
    /// Interaction logic for AddItemWindow.xaml
    /// </summary>
    public partial class AddItemDialog : Window
    {
        bool validName;
        bool validPrice;
        bool validCategory;

        public AddItemDialog()
        {
            InitializeComponent();
            OtherInitializeSetup();
        }

        private void OtherInitializeSetup()
        {
            addButton.IsEnabled = false;

            nameWarningTextBlock.Visibility = Visibility.Hidden;
            priceWarningTextBlock.Visibility = Visibility.Hidden;
            categoryWarningTextBlock.Visibility = Visibility.Hidden;

            validName = false;
            validPrice = false;
            validCategory = false;
        }

        internal String ItemName
        {
            get { return nameTextBox.Text; }
        }

        internal String ItemCategory
        {
            get { return (string)categoriesComboBox.SelectedItem; }
        }

        internal double ItemPrice
        {
            get { return Double.Parse(priceTextBox.Text); }
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

        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Double.TryParse(priceTextBox.Text, out double dump))
            {
                validPrice = true;
                priceWarningTextBlock.Visibility = Visibility.Hidden;
                UpdateAddButton();
            }
            else
            {
                validPrice = false;
                priceWarningTextBlock.Visibility = Visibility.Visible;
                UpdateAddButton();
            }
        }

        private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validCategory = true;
            UpdateAddButton();
        }

        private void UpdateAddButton()
        {
            if (validName && validPrice && validCategory)
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
