﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RestaurantPOS.Converters;
using RestaurantPOS.Models;
using System.Collections.ObjectModel;

namespace RestaurantPOS
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    internal Dictionary<Inventory, List<Item>> inventoryItemsDict;
    internal Dictionary<Tuple<string, string>, Item> itemNameCategoryObjectDict;
    internal Dictionary<string, List<Item>> categoryItemDict;

    public App()
    {
      Console.WriteLine("=================App started");
    }

    //must be build after inventoryPage.inventoryNameObjectDict
    internal void BuildInventoryItemsDict()
    {
      inventoryItemsDict = new Dictionary<Inventory, List<Item>>();
      Dictionary<String, Inventory> inventoryNameObjectDict = ((MainWindow)MainWindow).inventoryPage.inventoryNameObjectDict;
      foreach (Inventory inventory in ((MainWindow)MainWindow).inventoryPage.inventoryList)
      {
        inventoryItemsDict[inventory] = new List<Item>();
      }
      foreach (Item item in ((MainWindow)MainWindow).editPage.itemsList)
      {
        if (item.InventoryConsumptionList != null)
        {
          foreach(InventoryConsumption inventoryConsumption in item.InventoryConsumptionList)
          {
            inventoryItemsDict[inventoryNameObjectDict[inventoryConsumption.InventoryName]].Add(item);
          }
        }
      }
    }

    internal void BuildItemNameCategoryObjectDict()
    {

      itemNameCategoryObjectDict = new Dictionary<Tuple<string, string>, Item>();

      ObservableCollection<Item> itemsList = ((MainWindow)MainWindow).editPage.itemsList;
      foreach(Item item in itemsList)
      {
        itemNameCategoryObjectDict[new Tuple<string, string>(item.Name, item.Category)] = item;
      }
    }

    internal void BuildCategoryItemDict()
    {
      categoryItemDict = new Dictionary<string, List<Item>>();
      ObservableCollection<string> categoriesList = ((MainWindow)MainWindow).editPage.categoriesList;
      foreach (String category in categoriesList)
      {
        categoryItemDict.Add(category, new List<Item>());
      }
      ObservableCollection<Item> itemsList = ((MainWindow)MainWindow).editPage.itemsList;
      foreach (Item item in itemsList)
      {
        categoryItemDict[item.Category].Add(item);
        Console.WriteLine("Testing: item - " + item.Name + " category: " + item.Category);
      }

    }

    internal void AddCategoryToCategoryItemDict(string category)
    {
      categoryItemDict[category] = new List<Item>();
    }

    //make sure oldCategory!= newCategory
    internal void ModifyCategoryInCategoryItemDict(string oldCategory, string newCategory)
    {
      categoryItemDict[newCategory] = categoryItemDict[oldCategory];
      if (!newCategory.Equals(oldCategory)){
        categoryItemDict.Remove(oldCategory);
      }
    }

    internal void RemoveCategoryFromCategoryItemDict(string category)
    {
      categoryItemDict.Remove(category);
    }

    internal void AddItemToCategoryItemDict(string category, Item item)
    {
      categoryItemDict[category].Add(item);
    }

    internal void ChangeItemCategoryInCategoryItemDict(Item item, string oldCategory, string newCategory)
    {
      categoryItemDict[oldCategory].Remove(item);
      categoryItemDict[newCategory].Add(item);
    }

    internal void RemoveItemFromCategoryItemDict(Item item)
    {
      categoryItemDict[item.Category].Remove(item);
    }



    public Dictionary<string, List<Item>> CategoryItemDict
    {
      get { return this.categoryItemDict; }
    }
  }
}
