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

namespace RestaurantPOS.Pages
{
  /// <summary>
  /// Interaction logic for TestPage.xaml
  /// </summary>
  public partial class SelectionPage : UserControl
  {
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    WrapPanel categoriesWrapPanel;
    Dictionary<string,WrapPanel> categoryItemsWrapPanelDict;
    internal Circle tableUI;
    App currentApp = (App)Application.Current;

    SolidColorBrush antiqueWhiteBrush = new SolidColorBrush(Colors.AntiqueWhite);

    public SelectionPage()
    {
      InitializeComponent();
      Console.WriteLine("Selection Page created");
    }

    private void SelectionPage_Loaded(object sender, RoutedEventArgs e)
    {
      Console.WriteLine("=========SelectionPage_Loaded");
      
      if (tableUI != null)
      {
        TableNumberTextBlock.Text = "Table " + tableUI.Table.TableNumber;
        itemsListView.ItemsSource = tableUI.Table.DatabaseItemNameCategoryQuantityList; 
        wrapPanelScrollViewer.Content=categoriesWrapPanel;

        Binding myBinding = new Binding("Table.PriceTotal");
        myBinding.Source = tableUI;
        myBinding.StringFormat = "Total: {0:C2}";
        priceSummary.SetBinding(TextBlock.TextProperty, myBinding);
        
        //priceSummary.Text = "Total: $" +tableUI.Table.PriceTotal.ToString();
      }

      
      
      
      backToCategoriesButton.Visibility = Visibility.Hidden;
      DisableLeftButtons();

      goHomeButton.Focus();
      itemsListView.SelectedItem = null;
    }

    internal void BuildCategoryItemsWrapPanelDictionary()
    {
      categoryItemsWrapPanelDict = new Dictionary<string, WrapPanel>();
      ObservableCollection<string> categoriesList = currentApp.categoriesList;
      ObservableCollection<Item> itemsList = currentApp.itemsList;

      foreach (string categoryStr in categoriesList)
      {
        WrapPanel itemsWrapPanel = new WrapPanel();

        foreach (Item item in itemsList)
        {
          if (item.Category.Equals(categoryStr))
          {
            ItemButton itemButton = new ItemButton(item) {
              //Content = item.Name,
              Margin = new Thickness(10),
              Width = 150,
              Height = 100,
              Background = antiqueWhiteBrush
              //ButtonItem = item
            };
            

            itemButton.Click += ItemButton_Click;
            itemsWrapPanel.Children.Add(itemButton);
          }
        }
        categoryItemsWrapPanelDict.Add(categoryStr, itemsWrapPanel);
      }
    }

    internal void BuildCategoriesWrapPanel()
    {
      categoriesWrapPanel = new WrapPanel();
      ObservableCollection<string> categoriesList = currentApp.categoriesList;

      foreach (string categoryStr in categoriesList)
      {
        Button categoryButton = new Button
        {
          Content = categoryStr,
          Background = antiqueWhiteBrush,
          Margin = new Thickness(10),
          Width = 150,
          Height = 100
        };
        categoryButton.Click += CategoryButton_Click;
        categoriesWrapPanel.Children.Add(categoryButton);
      }
    }

    private void CategoryButton_Click(object sender, RoutedEventArgs e)
    {
      Console.WriteLine("=============CategoryButton Click");
      Button categoryButton = (Button)sender;
      WrapPanel itemsWrapPanel = categoryItemsWrapPanelDict[categoryButton.Content.ToString()];

      wrapPanelScrollViewer.Content = itemsWrapPanel;
      //currentCategory = categoryButton.Content.ToString();
      backToCategoriesButton.Visibility = Visibility.Visible;

    }

    private void ItemButton_Click(object sender, RoutedEventArgs e)
    {
      ItemButton itemButton = (ItemButton)sender;
      ObservableCollection<DatabaseItemNameCategoryQuantity> itemNameCategoryQuantityList = (ObservableCollection<DatabaseItemNameCategoryQuantity>)itemsListView.ItemsSource;

      bool newItemInTable = true;
      for (int i = 0; i < itemNameCategoryQuantityList.Count; i++)
      {
        
        if (itemNameCategoryQuantityList[i].ItemName.Equals(itemButton.Content.ToString())&&
          itemNameCategoryQuantityList[i].ItemCategory.Equals(itemButton.ButtonItem.Category) &&
          itemNameCategoryQuantityList[i].ItemPrice == itemButton.ButtonItem.Price)
        {
          newItemInTable = false;
          itemNameCategoryQuantityList[i].ItemQuantity++;
          itemNameCategoryQuantityList[i].ItemsPrice += itemButton.ButtonItem.Price;
          tableUI.Table.PriceTotal += itemButton.ButtonItem.Price;
          break;
        }
      }
      if (newItemInTable)
      {
        itemNameCategoryQuantityList.Add(new DatabaseItemNameCategoryQuantity {
          ItemName = itemButton.Content.ToString(),
          ItemCategory = itemButton.ButtonItem.Category,
          ItemQuantity = 1,
          ItemPrice = itemButton.ButtonItem.Price,
          ItemsPrice = itemButton.ButtonItem.Price
        });
        tableUI.Table.PriceTotal += itemButton.ButtonItem.Price;
      }
    }

    internal void AddCategoryToCategoriesWrapPanel(string categoryName)
    {
      WrapPanel ItemsWrapPanel = new WrapPanel();
      categoryItemsWrapPanelDict.Add(categoryName, ItemsWrapPanel);

      Button categoryButton = new Button {
        Content = categoryName,
        Background = antiqueWhiteBrush,
        Margin = new Thickness(10),
        Width = 150,
        Height = 100
      };
      categoryButton.Click += CategoryButton_Click;

      categoriesWrapPanel.Children.Add(categoryButton);
    }

    internal void ModifyCategoryInCategoryWrapPanel(string oldCategory, string newCategory)
    {
      for (int i = 0; i < categoriesWrapPanel.Children.Count; i++)
      {
        if (((Button)categoriesWrapPanel.Children[i]).Content.ToString().Equals(oldCategory))
        {
          ((Button)categoriesWrapPanel.Children[i]).Content = newCategory;
          break;
        }
      }
      categoryItemsWrapPanelDict[newCategory] = categoryItemsWrapPanelDict[oldCategory];
      categoryItemsWrapPanelDict.Remove(oldCategory);
    }

    internal void RemoveCategoryToCategoriesWrapPanel(string categoryName)
    {
      categoryItemsWrapPanelDict.Remove(categoryName);
      for (int i=0; i< categoriesWrapPanel.Children.Count; i++)
      {
        if (((Button)categoriesWrapPanel.Children[i]).Content.ToString().Equals(categoryName))
        {
          categoriesWrapPanel.Children.Remove(categoriesWrapPanel.Children[i]);
          break;
        }
      }
    }

    internal void AddItemToItemsWrapPanel(Item item)
    {
      WrapPanel itemsWrapPanel = categoryItemsWrapPanelDict[item.Category];

      ItemButton itemButton = new ItemButton(item) {
        //Content = item.Name,
        Background = antiqueWhiteBrush,
        Margin = new Thickness(10),
        Width = 150,
        Height = 100
        //ButtonItem = item
      };
      itemButton.Click += ItemButton_Click;
      itemsWrapPanel.Children.Add(itemButton);
    }

    internal void RemoveItemFromItemsWrapPanel(Item item)
    {
      WrapPanel itemsWrapPanel = categoryItemsWrapPanelDict[item.Category];

      for (int i = 0; i < itemsWrapPanel.Children.Count; i++)
      {
        if (((Button)itemsWrapPanel.Children[i]).Content.Equals(item.Name))
        {
          itemsWrapPanel.Children.Remove(itemsWrapPanel.Children[i]);
        }
      }
    }

    

    private void DisableLeftButtons()
    {
      removeItemsButton.IsEnabled = false;
      addItemButton.IsEnabled = false;
      minusItemButton.IsEnabled = false;
    }


    private void RightGrid_GotFocus(object sender, RoutedEventArgs e)
    {
      //Console.WriteLine("=======SelectionPage_RightGrid_GotFocus");
      DisableLeftButtons();
    }

    private void LeftGrid_LostFocus(object sender, RoutedEventArgs e)
    {
      //Console.WriteLine("=======LeftGrid_LostFocus");
    }

    private void LeftGrid_GotFocus(object sender, RoutedEventArgs e)
    {
      //Console.WriteLine("=======LeftGrid_GotFocus");
    }

    private void ItemsListView_LostFocus(object sender, RoutedEventArgs e)
    {
     //Console.WriteLine("====ItemsListView_LostFocus");
    }

    private void ItemsListView_GotFocus(object sender, RoutedEventArgs e)
    {
      //Console.WriteLine("====ItemsListView_GotFocus");
      DetermineLeftGridButtons();
    }

    private void ItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      Console.WriteLine("======itemsListView_SelectionChanged");
      DetermineLeftGridButtons();
    }

    private void DetermineLeftGridButtons()
    {
      if (itemsListView.SelectedItem != null)
      {
        removeItemsButton.IsEnabled = true;
        addItemButton.IsEnabled = true;
        DatabaseItemNameCategoryQuantity selectedItem = (DatabaseItemNameCategoryQuantity)itemsListView.SelectedItem;
        if (selectedItem.ItemQuantity > 1)
        {
          minusItemButton.IsEnabled = true;
        }
        else
        {
          minusItemButton.IsEnabled = false;
        }
      }
    }

    private void RemoveItemsButton_Click(object sender, RoutedEventArgs e)
    {      
      YesNoCancelDialog yesNoCancelDialog = new YesNoCancelDialog("Do you want to Remove this Item");
      if (yesNoCancelDialog.ShowDialog() == true)
      {
        DatabaseItemNameCategoryQuantity selectedItem = (DatabaseItemNameCategoryQuantity)itemsListView.SelectedItem;
        tableUI.Table.PriceTotal -= selectedItem.ItemsPrice;
        ((ObservableCollection<DatabaseItemNameCategoryQuantity>)itemsListView.ItemsSource).Remove(selectedItem);
        
        DisableLeftButtons();
      }
      else
      {
        itemsListView.Focus();
      }   
    }

    private void MinusItemButton_Click(object sender, RoutedEventArgs e)
    {
      DatabaseItemNameCategoryQuantity selectedItem = (DatabaseItemNameCategoryQuantity)itemsListView.SelectedItem;
      tableUI.Table.PriceTotal -= (selectedItem.ItemsPrice / selectedItem.ItemQuantity);
      selectedItem.ItemsPrice -= (selectedItem.ItemsPrice / selectedItem.ItemQuantity);
      
      selectedItem.ItemQuantity--;
      if (selectedItem.ItemQuantity == 1)
      {
        minusItemButton.IsEnabled = false;
      }
      itemsListView.Focus();
      
    }

    private void AddItemButton_Click(object sender, RoutedEventArgs e)
    {
      DatabaseItemNameCategoryQuantity selectedItem = (DatabaseItemNameCategoryQuantity)itemsListView.SelectedItem;
      tableUI.Table.PriceTotal += (selectedItem.ItemsPrice / selectedItem.ItemQuantity);
      selectedItem.ItemsPrice += (selectedItem.ItemsPrice / selectedItem.ItemQuantity);
      
      selectedItem.ItemQuantity++;
      itemsListView.Focus();
    }

    private void BackToCategoriesButton_Click(object sender, RoutedEventArgs e)
    {
      wrapPanelScrollViewer.Content = categoriesWrapPanel;
      backToCategoriesButton.Visibility = Visibility.Hidden;
    }

    private void GoHomeButton_Click(object sender, RoutedEventArgs e)
    {
      mainWindow.tabControl.SelectedItem = mainWindow.tablesTab;
    }

    private void CheckButton_Click(object sender, RoutedEventArgs e)
    {
      YesNoCancelDialog yesNoCancelDialog = new YesNoCancelDialog("The Total is "+ tableUI.Table.PriceTotal+ ". Are you you sure to give the Check?");
      if (yesNoCancelDialog.ShowDialog() == true)
      {
        tableUI.Table.IsActive = false;
        tableUI.circleUI.Stroke = null;
        tableUI.Table.PriceTotal = 0;

        Dictionary<string, Item> itemNameObjectDict = currentApp.ItemNameObjectDict;
        Dictionary<string, Inventory> inventoryNameObjectDict = currentApp.InventoryNameObjectDict;
        foreach (DatabaseItemNameCategoryQuantity itemCategoryQuantity in tableUI.Table.DatabaseItemNameCategoryQuantityList)
        {
          if (itemNameObjectDict.ContainsKey(itemCategoryQuantity.itemName))
          {
            Item consumedItem = itemNameObjectDict[itemCategoryQuantity.itemName];
            foreach (InventoryConsumption inventoryConsumption in consumedItem.InventoryConsumptionList)
            {
              if (inventoryNameObjectDict.ContainsKey(inventoryConsumption.InventoryName))
              {
                inventoryNameObjectDict[inventoryConsumption.InventoryName].Quantity -= (inventoryConsumption.ConsumptionQuantity* itemCategoryQuantity.ItemQuantity);
              }  
            }
          }       
        }
        tableUI.Table.DatabaseItemNameCategoryQuantityList = new ObservableCollection<DatabaseItemNameCategoryQuantity>();
        mainWindow.tabControl.SelectedItem = mainWindow.tablesTab;
        mainWindow.selectionPageTab.IsEnabled = false;
        //update dictionaries


      }

    }

  }



  
}