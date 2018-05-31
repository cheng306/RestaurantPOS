using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RestaurantPOS.Converters;
using RestaurantPOS.Models;
using System.Collections.ObjectModel;
using RestaurantPOS.Pages;

namespace RestaurantPOS
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    Dictionary<string, List<Item>> inventoryNameItemsListDict;
    //internal Dictionary<Tuple<string, string>, Item> itemNameCategoryObjectDict;
    internal Dictionary<string, List<Item>> categoryItemsListDict;

   
    
    public App()
    {
      //mainWindow = ((MainWindow)Application.Current.MainWindow);
      Console.WriteLine("=================App started");
    }

    

    //internal void BuildItemNameCategoryObjectDict()
    //{
    //  itemNameCategoryObjectDict = new Dictionary<Tuple<string, string>, Item>();

    //  ObservableCollection<Item> itemsList = ((MainWindow)Application.Current.MainWindow).editPage.itemsList;
    //  foreach(Item item in itemsList)
    //  {
    //    itemNameCategoryObjectDict[new Tuple<string, string>(item.Name, item.Category)] = item;
    //  }
    //}

    internal void BuildCategoryItemDict()
    {
      categoryItemsListDict = new Dictionary<string, List<Item>>();
      ObservableCollection<string> categoriesList = ((MainWindow)Application.Current.MainWindow).editPage.categoriesList;
      foreach (String category in categoriesList)
      {
        categoryItemsListDict.Add(category, new List<Item>());
      }
      ObservableCollection<Item> itemsList = ((MainWindow)Application.Current.MainWindow).editPage.itemsList;
      foreach (Item item in itemsList)
      {
        categoryItemsListDict[item.Category].Add(item);
        Console.WriteLine("Testing: item - " + item.Name + " category: " + item.Category);
      }

    }

    internal void AddCategoryToCategoryItemDict(string category)
    {
      categoryItemsListDict[category] = new List<Item>();
    }

    //make sure oldCategory!= newCategory
    internal void ModifyCategoryInCategoryItemDict(string oldCategory, string newCategory)
    {
      categoryItemsListDict[newCategory] = categoryItemsListDict[oldCategory];
      if (!newCategory.Equals(oldCategory)){
        categoryItemsListDict.Remove(oldCategory);
      }
    }

    internal void RemoveCategoryFromCategoryItemDict(string category)
    {
      categoryItemsListDict.Remove(category);
    }

    internal void AddItemToCategoryItemDict(string category, Item item)
    {
      categoryItemsListDict[category].Add(item);
    }

    internal void ChangeItemCategoryInCategoryItemDict(Item item, string oldCategory, string newCategory)
    {
      categoryItemsListDict[oldCategory].Remove(item);
      categoryItemsListDict[newCategory].Add(item);
    }

    internal void RemoveItemFromCategoryItemDict(Item item)
    {
      categoryItemsListDict[item.Category].Remove(item);
    }

    public Dictionary<string, List<Item>> CategoryItemsDict
    {
      get { return this.categoryItemsListDict; }
    }
    //above are about categoryItemsListDict

    public void BuildInventoryNameItemsListDict()
    {
      inventoryNameItemsListDict = new Dictionary<string, List<Item>>();
      foreach (Inventory inventory in ((MainWindow)Application.Current.MainWindow).inventoryPage.inventoryList)
      {
        inventoryNameItemsListDict[inventory.Name] = new List<Item>();
      }
      foreach (Item item in ((MainWindow)Application.Current.MainWindow).editPage.itemsList)
      {
        foreach (InventoryConsumption inventoryConsumption in item.InventoryConsumptionList)
        {
          inventoryNameItemsListDict[inventoryConsumption.InventoryName].Add(item);
        }
      }
    }

    public void AddInventoryToInventoryNameItemsListDict(Inventory inventory)
    {
      inventoryNameItemsListDict[inventory.Name] = new List<Item>();
    }

    public void EditInventoryNameInInventoryNameItemsListDict(string oldInventoryName, string newInventoryName)
    {
      inventoryNameItemsListDict[newInventoryName] = inventoryNameItemsListDict[oldInventoryName];
      inventoryNameItemsListDict.Remove(oldInventoryName);
    }

    public void RemoveInventoryFromInventoryNameItemsListDict(Inventory inventory)
    {
      inventoryNameItemsListDict.Remove(inventory.Name);
    }

    public void AddItemToInventoryNameItemsListDict(Item item, string inventoryName)
    {
      inventoryNameItemsListDict[inventoryName].Add(item);
    }

    public void RemoveItemFromInventoryNameItemsListDict(Item item)
    {
      foreach (InventoryConsumption inventoryConsumption in item.InventoryConsumptionList)
      {
        inventoryNameItemsListDict[inventoryConsumption.InventoryName].Remove(item);
      }
    }

    public Dictionary<string,List<Item>> InventoryNameItemsListDict
    {
      get { return this.inventoryNameItemsListDict; }
    }

    //must be build after inventoryPage.inventoryNameObjectDict
    //internal void BuildInventoryItemsDict()
    //{
    //  inventoryItemsDict = new Dictionary<Inventory, List<Item>>();
    //  Dictionary<String, Inventory> inventoryNameObjectDict = ((MainWindow)MainWindow).inventoryPage.inventoryNameObjectDict;
    //  foreach (Inventory inventory in ((MainWindow)MainWindow).inventoryPage.inventoryList)
    //  {
    //    inventoryItemsDict[inventory] = new List<Item>();
    //  }
    //  foreach (Item item in ((MainWindow)MainWindow).editPage.itemsList)
    //  {
    //    if (item.InventoryConsumptionList != null)
    //    {
    //      foreach (InventoryConsumption inventoryConsumption in item.InventoryConsumptionList)
    //      {
    //        inventoryItemsDict[inventoryNameObjectDict[inventoryConsumption.InventoryName]].Add(item);
    //      }
    //    }
    //  }
    //}


  }
}
