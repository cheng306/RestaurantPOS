using RestaurantPOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantPOS.Dictionaries
{
  internal class CategoryItemsWrapPanelDict:Dictionary<string,WrapPanel>
  {
    static App currentApp = (App)Application.Current;
    ObservableCollection<string> categoriesList = currentApp.CategoriesList;
    ObservableCollection<Item> itemsList = currentApp.itemsList;
    internal CategoryItemsWrapPanelDict()
    {
      foreach(string category in categoriesList)
      {
        this[category] = new WrapPanel();
      }
      foreach(Item item in itemsList)
      {
        ItemButton itemButton = new ItemButton(item);
        this[item.Category].Children.Add(itemButton);
      }
    }
  }
}
