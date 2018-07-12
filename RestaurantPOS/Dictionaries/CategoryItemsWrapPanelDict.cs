using RestaurantPOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RestaurantPOS.CustomControls;
using RestaurantPOS.SerializedData;

namespace RestaurantPOS.Dictionaries
{
  internal class CategoryItemsWrapPanelDict:Dictionary<string,WrapPanel>
  {
    static App currentApp = (App)Application.Current;
    ObservableCollection<string> categoriesList = currentApp.CategoriesList;
    ItemsList itemsList = currentApp.ItemsList;
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

    internal void AddCategoryToCategoryItemsWrapPanelDict(string categoryName)
    {
      WrapPanel ItemsWrapPanel = new WrapPanel();
      this.Add(categoryName, ItemsWrapPanel);
    }

    internal void ModifyCategoryInCategoryItemsWrapPanelDict(string oldCategory, string newCategory)
    {
      this[newCategory] = this[oldCategory];
      this.Remove(oldCategory);
    }

    internal void RemoveCategoryFromCategoryItemsWrapPanelDict(string category)
    {
      this.Remove(category);
    }

    internal void AddItemToItemsWrapPanel(Item item)
    {
      WrapPanel itemsWrapPanel = this[item.Category];
      ItemButton itemButton = new ItemButton(item);

      itemsWrapPanel.Children.Add(itemButton);
    }

    internal void RemoveItemFromItemsWrapPanel(Item item)
    {
      WrapPanel itemsWrapPanel = this[item.Category];

      for (int i = 0; i < itemsWrapPanel.Children.Count; i++)
      {
        if (((Button)itemsWrapPanel.Children[i]).Content.Equals(item.Name))
        {
          itemsWrapPanel.Children.Remove(itemsWrapPanel.Children[i]);
        }
      }
    }




  }
}
