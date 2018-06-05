using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RestaurantPOS.Models;

namespace RestaurantPOS.Dictionaries
{
  class ItemNameObjectDict : Dictionary<string,Item>
  {

    public ItemNameObjectDict()
    {
      foreach (Item item in (((App)Application.Current).itemsList))
      {
        this[item.Name] = item;
      }
    }

    internal void AddItemToItemNameObjectDict(Item item)
    {
      this[item.Name] = item;
    }

    internal void EditItemNameInItemNameObjectDict(string oldName, string newName)
    {
      this[newName] = this[oldName];
      this.Remove(oldName);
    }

    internal void RemoveItemFromItemNameObjectDict(Item item)
    {
      this.Remove(item.Name);
    }

    internal void RemoveItemFromItemNameObjectDict(string itemName)
    {
      this.Remove(itemName);
    }
  }
}
