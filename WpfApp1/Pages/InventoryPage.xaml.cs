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
using RestaurantPOS.Models;
using RestaurantPOS.Dialogs;
using RestaurantPOS.Dialogs.Templates;

namespace RestaurantPOS.Pages
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class InventoryPage : UserControl
    {
        internal ObservableCollection<Inventory> inventoryList;

        // indicators that if the listviewgot focus before 
        bool leftEnabled;
        bool rightEnabled;

        public InventoryPage()
        {
            InitializeComponent();
        }

        private void InventoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            itemsListView.SelectedItem = null;
            inventoryListView.SelectedItem = null;
            editItemConsumptionButton.IsEnabled = false;
            DisableRightGridButtons();
            leftEnabled = false;
            rightEnabled = false;
        }

        private void EditItemConsumptionButton_Click(object sender, RoutedEventArgs e)
        {
            Item seletedItem = (Item)itemsListView.SelectedItem;
            EditItemInventoryDialog editItemInventoryDialog = new EditItemInventoryDialog(seletedItem);
                
            if (editItemInventoryDialog.ShowDialog() == true)//when user click confrim button
            {
                List<InventoryConsumption> inventoryConsumptionList = new List<InventoryConsumption>();
                foreach (DependencyRow dependencyRow in editItemInventoryDialog.dependenciesStackpanel.Children)
                {
                    InventoryConsumption inventoryConsumption = new InventoryConsumption{
                        InventoryName = dependencyRow.InventoryName,
                        ConsumptionQuantity = dependencyRow.ConsumptionQuantity
                    };
                    inventoryConsumptionList.Add(inventoryConsumption);
                    Console.WriteLine(dependencyRow.inventoryComboBox.SelectedValue + " " + dependencyRow.quantityTextBox.Text);
                }
                seletedItem.InventoryConsumptionList = inventoryConsumptionList;
            }
        }

        private void CreateInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddInventoryWindow addInventoryWindow = new AddInventoryWindow();
            if (addInventoryWindow.ShowDialog() == true)
            {
                Inventory addedInventory = new Inventory { Name = addInventoryWindow.InventoryName, Quantity = addInventoryWindow.InventoryQuantity };
                inventoryList.Add(addedInventory);

                inventoryListView.SelectedItem = addedInventory;
                inventoryListView.Focus();
                rightEnabled = true;
            }
            else
            {
                if (rightEnabled)
                {
                    inventoryListView.Focus();
                }

            }
        }

        private void ModifyInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            Inventory selectedInventory = (Inventory)inventoryListView.SelectedItem;

            AddInventoryWindow addInventoryWindow = new AddInventoryWindow();
            addInventoryWindow.addButton.IsEnabled = true;
            addInventoryWindow.Title = "Modify Inventory";
            addInventoryWindow.addButton.Content = "Modify";
            addInventoryWindow.nameTextBox.Text = selectedInventory.Name;
            addInventoryWindow.quantityTextBox.Text = selectedInventory.Quantity.ToString();
            if (addInventoryWindow.ShowDialog() == true)
            {
                selectedInventory.Name = addInventoryWindow.InventoryName;      
                selectedInventory.Quantity = addInventoryWindow.InventoryQuantity;
            }

            inventoryListView.Focus();
        }

        private void RemoveInventoryButton_Click(object sender, RoutedEventArgs e)
        {
            inventoryList.Remove((Inventory)inventoryListView.SelectedItem);

            DisableRightGridButtons();
            rightEnabled = false;
        }
        
        private void LeftGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            DisableRightGridButtons();
            rightEnabled = false;
        }

        private void RightGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            editItemConsumptionButton.IsEnabled = false;
            leftEnabled = false;
        }

        private void InventoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableRightGridButtons();
            rightEnabled = true;
        }

        private void ItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            editItemConsumptionButton.IsEnabled = true;
            leftEnabled = true;
        }

        private void ItemsListView_GotFocus(object sender, RoutedEventArgs e)
        {
            if (itemsListView.SelectedItem != null)
            {
                editItemConsumptionButton.IsEnabled = true;
                leftEnabled = true;
            }
        }

        private void InventoryListView_GotFocus(object sender, RoutedEventArgs e)
        {
            if (inventoryListView.SelectedItem != null)
            {
                EnableRightGridButtons();
                rightEnabled = true;
            }
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

    }
}
