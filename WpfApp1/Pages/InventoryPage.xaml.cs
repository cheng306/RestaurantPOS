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
    internal Dictionary<String, Inventory> inventoryNameObjectDict;

    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    //Dictionary<Inventory, List<Item>> inventoryItemsDict = ((App)Application.Current).inventoryItemsDict;

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

    internal void BuildNameInventoryDict()
    {
      inventoryNameObjectDict = new Dictionary<string, Inventory>();
      foreach (Inventory inventory in inventoryList)
      {
        inventoryNameObjectDict.Add(inventory.Name, inventory);
      }
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
          InventoryConsumption inventoryConsumption = new InventoryConsumption
          {
            InventoryName = dependencyRow.InventoryName,
            ConsumptionQuantity = dependencyRow.ConsumptionQuantity
          };
          inventoryConsumptionList.Add(inventoryConsumption);
          Console.WriteLine(dependencyRow.inventoryComboBox.SelectedValue + " " + dependencyRow.quantityTextBox.Text);
        }

        seletedItem.InventoryConsumptionList = inventoryConsumptionList;

      }
      else
      {
        itemsListView.Focus();
      }
    }

    private void CreateInventoryButton_Click(object sender, RoutedEventArgs e)
    {
      AddInventoryDialog addInventoryWindow = new AddInventoryDialog();
      if (addInventoryWindow.ShowDialog() == true)
      {
        Inventory addedInventory = new Inventory
        {
          Name = addInventoryWindow.InventoryName,
          Quantity = addInventoryWindow.InventoryQuantity,
          Unit = addInventoryWindow.InventoryUnit
        };

        inventoryNameObjectDict.Add(addedInventory.Name, addedInventory);
        inventoryList.Add(addedInventory);

        inventoryListView.SelectedItem = addedInventory;
        inventoryListView.Focus();
        rightEnabled = true;
      }
      else //did not add a new inventory
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

      AddInventoryDialog addInventoryWindow = new AddInventoryDialog(selectedInventory);
      addInventoryWindow.addButton.IsEnabled = true;
      addInventoryWindow.Title = "Modify Inventory";
      addInventoryWindow.addButton.Content = "Modify";
      addInventoryWindow.nameTextBox.Text = selectedInventory.Name;
      addInventoryWindow.quantityTextBox.Text = selectedInventory.Quantity.ToString();
      addInventoryWindow.unitTextBox.Text = selectedInventory.Unit;

      if (addInventoryWindow.ShowDialog() == true)
      {
        string oldInventoryName = selectedInventory.Name;
        string newInventoryName = addInventoryWindow.InventoryName;
        if (!oldInventoryName.Equals(newInventoryName))
        {
          UpdateItemInventoryConsumptionList(oldInventoryName, newInventoryName);
          UpdateInventoryNameObjectDict(oldInventoryName, newInventoryName);
        }

        selectedInventory.Name = addInventoryWindow.InventoryName;
        selectedInventory.Quantity = addInventoryWindow.InventoryQuantity;
        selectedInventory.Unit = addInventoryWindow.InventoryUnit;
      }

      inventoryListView.Focus();
    }

    //helper method for ModifyInventoryButton_Click()
    private void UpdateItemInventoryConsumptionList(string oldInventoryName, string newInventoryName)
    {
      Dictionary<Inventory, List<Item>> inventoryItemsDict = ((App)Application.Current).inventoryItemsDict;
      List<Item> itemsList = inventoryItemsDict[inventoryNameObjectDict[oldInventoryName]];
      foreach (Item item in itemsList)
      {
        if (item.InventoryConsumptionList != null)
        {
          for (int i = 0; i < item.InventoryConsumptionList.Count; i++)
          {
            if (item.InventoryConsumptionList[i].InventoryName.Equals(oldInventoryName))
            {
              item.InventoryConsumptionList[i].InventoryName = newInventoryName;
              break;
            }
          }
        }
      }
    }

    private void UpdateInventoryNameObjectDict(string oldInventoryName, string newInventoryName){
      inventoryNameObjectDict[newInventoryName] = inventoryNameObjectDict[oldInventoryName];
      inventoryNameObjectDict.Remove(oldInventoryName);
    }

    private void RemoveInventoryButton_Click(object sender, RoutedEventArgs e)
    {
      Inventory selectedInventory = (Inventory)inventoryListView.SelectedItem;
      RemoveItemInventoryConsumptionFromItemInventoryConsumptionList(selectedInventory);
      inventoryNameObjectDict.Remove(selectedInventory.Name);
      inventoryList.Remove(selectedInventory);

      DisableRightGridButtons();
      rightEnabled = false;
    }

    //helper method of RemoveInventoryButton_Click()
    private void RemoveItemInventoryConsumptionFromItemInventoryConsumptionList(Inventory selectedInventory)
    {
      Dictionary<Inventory, List<Item>> inventoryItemsDict = ((App)Application.Current).inventoryItemsDict;
      List<Item> itemsList = inventoryItemsDict[selectedInventory];
      foreach(Item item in itemsList)
      {
        if (item.InventoryConsumptionList != null)
        {
          for (int i =0; i< item.InventoryConsumptionList.Count; i++)
          {
            if (item.InventoryConsumptionList[i].InventoryName.Equals(selectedInventory.Name))
            {
              item.InventoryConsumptionList.Remove(item.InventoryConsumptionList[i]);
              break;
            }
          }
        }
      }
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
