using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RestaurantPOS.Models;
using RestaurantPOS.Pages;

namespace RestaurantPOS.Dictionaries
{
  class InventoryNameObjectDict : Dictionary<string, Inventory>
  {
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
  

    public InventoryNameObjectDict()
    {
      foreach (Inventory inventory in mainWindow.inventoryPage.inventoryList)
      {
        this[inventory.Name] = inventory;
      } 
    }

    internal void AddInventoryToInventoryNameObjectDict(Inventory inventory)
    {
      this[inventory.Name] = inventory;
    }

    internal void UpdateInventoryNameObjectDict(string oldInventoryName, string newInventoryName)
    {
      this[newInventoryName] = this[oldInventoryName];
      this.Remove(oldInventoryName);
    }

    internal void RemoveInventoryFromInventoryNameObjectDict(Inventory inventory)
    {
      this.Remove(inventory.Name);
    }

    internal void RemoveInventoryFromInventoryNameObjectDict(string inventoryName)
    {
      this.Remove(inventoryName);
    }
    
  }
}
