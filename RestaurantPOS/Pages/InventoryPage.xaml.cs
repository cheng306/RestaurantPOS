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
using RestaurantPOS.Models;
using RestaurantPOS.Dialogs;
using RestaurantPOS.Dictionaries;
using RestaurantPOS.Dialogs.Templates;

namespace RestaurantPOS.Pages
{
  /// <summary>
  /// Interaction logic for Inventory.xaml
  /// </summary>
  public partial class InventoryPage : UserControl
  {
    ObservableCollection<Item> itemsList;
    ObservableCollection<Inventory> inventoryList;
    InventoryNameObjectDict inventoryNameObjectDict;

    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    App currentApp = (App)Application.Current; 
    //Dictionary<Inventory, List<Item>> inventoryItemsDict = ((App)Application.Current).inventoryItemsDict;

    // indicators that if the listviewgot focus before 
    bool leftEnabled;
    bool rightEnabled;

    public InventoryPage()
    {
      InitializeComponent();

      itemsList = currentApp.ItemsList;
      itemsListView.ItemsSource = currentApp.ItemsList;

      inventoryList = currentApp.inventoryList;
      inventoryListView.ItemsSource = inventoryList;

      inventoryNameObjectDict = currentApp.InventoryNameObjectDict;

      Console.WriteLine("======InventoryPage Created");
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
      Item selectedItem = (Item)itemsListView.SelectedItem;
      EditItemInventoryDialog editItemInventoryDialog = new EditItemInventoryDialog(selectedItem);
      
      if (editItemInventoryDialog.ShowDialog() == true)//when user click confrim button
      {
        //modify the inventoryItemsDict
        currentApp.RemoveItemFromInventoryNameItemsListDict(selectedItem);

        List<InventoryConsumption> inventoryConsumptionList = new List<InventoryConsumption>();
        foreach (DependencyRow dependencyRow in editItemInventoryDialog.dependenciesStackpanel.Children)
        {
          InventoryConsumption inventoryConsumption = new InventoryConsumption
          {
            InventoryName = dependencyRow.InventoryName,
            ConsumptionQuantity = dependencyRow.ConsumptionQuantity
          };
          inventoryConsumptionList.Add(inventoryConsumption);

          currentApp.AddItemToInventoryNameItemsListDict(selectedItem, dependencyRow.InventoryName);
          
        }

        selectedItem.InventoryConsumptionList = inventoryConsumptionList;
        itemsListView.Focus();
      }
      else
      {
        itemsListView.Focus();
      }
    }

    private void CreateInventoryButton_Click(object sender, RoutedEventArgs e)
    {
      EditInventoryDialog addInventoryWindow = new EditInventoryDialog();
      if (addInventoryWindow.ShowDialog() == true)
      {
        Inventory addedInventory = new Inventory
        {
          Name = addInventoryWindow.InventoryName,
          Quantity = addInventoryWindow.InventoryQuantity,
          Unit = addInventoryWindow.InventoryUnit,
          RemindingLevel = addInventoryWindow.InventoryRemindingLevel
        };

        //update InventoryNameObjectDict
        inventoryNameObjectDict.AddInventoryToInventoryNameObjectDict(addedInventory);

        //update InventoryNameItemsListDict
        currentApp.AddInventoryToInventoryNameItemsListDict(addedInventory);

        //update inventoryList
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

      EditInventoryDialog editInventoryWindow = new EditInventoryDialog(selectedInventory);
      if (editInventoryWindow.ShowDialog() == true)
      {
        string oldInventoryName = selectedInventory.Name;
        string newInventoryName = editInventoryWindow.InventoryName;
        if (!oldInventoryName.Equals(newInventoryName))
        {
          UpdateInventoryNameInItemInventoryConsumptionList(oldInventoryName, newInventoryName);
          
          //update inventoryNameObjectDict
          currentApp.InventoryNameObjectDict.UpdateInventoryNameObjectDict(oldInventoryName, newInventoryName);

          //update InventoryNameItemsListDict
          currentApp.EditInventoryNameInInventoryNameItemsListDict(oldInventoryName, newInventoryName);
        }

        selectedInventory.Name = editInventoryWindow.InventoryName;
        selectedInventory.Quantity = editInventoryWindow.InventoryQuantity;
        selectedInventory.Unit = editInventoryWindow.InventoryUnit;
        selectedInventory.RemindingLevel = editInventoryWindow.InventoryRemindingLevel;
      }

      inventoryListView.Focus();
    }

    private void RemoveInventoryButton_Click(object sender, RoutedEventArgs e)
    {
      Inventory selectedInventory = (Inventory)inventoryListView.SelectedItem;

      string removeInventoryMessage = "Are you sure to remove "+ selectedInventory.Name + " from Inventory";
      YesNoCancelDialog yesNoCancelDialog = new YesNoCancelDialog(removeInventoryMessage);
      if (yesNoCancelDialog.ShowDialog() == true)
      {
        RemoveInventoryFromInventoryConsumptionLists(selectedInventory);

        //update InventoryNameItemListDict
        currentApp.RemoveInventoryFromInventoryNameItemsListDict(selectedInventory);

        //update InventoryNameObjectDict
        currentApp.InventoryNameObjectDict.RemoveInventoryFromInventoryNameObjectDict(selectedInventory);

        inventoryList.Remove(selectedInventory);
      }
      

      DisableRightGridButtons();
      rightEnabled = false;
    }

    //helper method for ModifyInventoryButton_Click()
    private void UpdateInventoryNameInItemInventoryConsumptionList(string oldInventoryName, string newInventoryName)
    {
      //Dictionary<Inventory, List<Item>> inventoryItemsDict = ((App)Application.Current).inventoryItemsDict;
      List<Item> itemsList = currentApp.InventoryNameItemsListDict[oldInventoryName];
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

    //helper method of RemoveInventoryButton_Click()
    private void RemoveInventoryFromInventoryConsumptionLists(Inventory selectedInventory)
    {
      List<Item> itemsList = currentApp.InventoryNameItemsListDict[selectedInventory.Name];
      foreach (Item item in itemsList)
      {
        if (item.InventoryConsumptionList != null)
        {
          for (int i = 0; i < item.InventoryConsumptionList.Count; i++)
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
