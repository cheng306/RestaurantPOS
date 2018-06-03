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
    public InventoryNameObjectDict()
    {
      foreach (Inventory inventory in ((App)Application.Current).inventoryList)
      {
        this[inventory.Name] = inventory;
      } 
    }

    internal void AddInventoryToInventoryNameObjectDict(Inventory inventory)
    {
      this[inventory.Name] = inventory;
      Console.WriteLine("Count: "+this.Count);
      Console.WriteLine("InventoryNameObjectDict[{0}]: {1}", inventory.Name, this[inventory.Name]);
    }

    internal void UpdateInventoryNameObjectDict(string oldInventoryName, string newInventoryName)
    {
      this[newInventoryName] = this[oldInventoryName];
      this.Remove(oldInventoryName);
      Console.WriteLine("Count: " + this.Count);
      Console.WriteLine("InventoryNameObjectDict[{0}]: {1}", newInventoryName, this[newInventoryName]);
    }

    internal void RemoveInventoryFromInventoryNameObjectDict(Inventory inventory)
    {
      this.Remove(inventory.Name);
      Console.WriteLine("Count: " + this.Count);
    }

    internal void RemoveInventoryFromInventoryNameObjectDict(string inventoryName)
    {
      this.Remove(inventoryName);
      Console.WriteLine("Count: " + this.Count);
    }
    
  }
}
