using System;
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
using WpfApp1.Models;
using WpfApp1.Windows;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class InventoryPage : UserControl
    {
        internal ObservableCollection<Inventory> inventoryList;

        public InventoryPage()
        {
            InitializeComponent();
        }

        private void InventoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            modifyInventoryButton.IsEnabled = false;
            removeInventoryButton.IsEnabled = false;
        }

        private void CreateInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddInventoryWindow addInventoryWindow = new AddInventoryWindow();
            if (addInventoryWindow.ShowDialog() == true)
            {
                Console.WriteLine("===========new inventory created");
                inventoryList.Add(new Inventory {Name=addInventoryWindow.InventoryName, Quantity=addInventoryWindow.InventoryQuantity});
            }
        }

        private void ModifyInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            Inventory selectedInventory = (Inventory)inventoryListView.SelectedItem;

            AddInventoryWindow addInventoryWindow = new AddInventoryWindow();
            addInventoryWindow.addButton.IsEnabled = true;
            addInventoryWindow.Title = "Modify Inventory";
            addInventoryWindow.nameTextBox.Text = selectedInventory.Name;
            addInventoryWindow.quantityTextBox.Text = selectedInventory.Quantity.ToString();
            if (addInventoryWindow.ShowDialog() == true)
            {
                Console.WriteLine("=============reach");
                selectedInventory.Name = addInventoryWindow.InventoryName;
            
                selectedInventory.Quantity = addInventoryWindow.InventoryQuantity;
                Console.WriteLine(addInventoryWindow.InventoryQuantity);
            }

            //DisableRightGridButtons();

        }

        private void RemoveInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            inventoryList.Remove((Inventory)inventoryListView.SelectedItem);

            DisableRightGridButtons();
        }

        

        private void RightGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            //modifyInventoryButton.IsEnabled = false;
            //removeInventoryButton.IsEnabled = false;
        }

        private void InventoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            modifyInventoryButton.IsEnabled = true;
            removeInventoryButton.IsEnabled = true;
            Console.WriteLine("Selection Changed");
        }

        private void LeftGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            DisableRightGridButtons();
        }

        private void EnableRightGridButtons()
        {
            modifyInventoryButton.IsEnabled = true;
            removeInventoryButton.IsEnabled = true;
        }

        private void DisableRightGridButtons()
        {
            modifyInventoryButton.IsEnabled = false;
            removeInventoryButton.IsEnabled = false;
        }

        private void EditItemConsumptionButton_Click(object sender, RoutedEventArgs e)
        {
            AddInventoryConsumptionWindow addInventoryConsumptionWindow = new AddInventoryConsumptionWindow();
            addInventoryConsumptionWindow.ShowDialog();
        }
    }
}
