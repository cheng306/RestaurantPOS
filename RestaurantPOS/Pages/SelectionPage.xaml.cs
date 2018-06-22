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
using RestaurantPOS.Dictionaries;

namespace RestaurantPOS.Pages
{
  /// <summary>
  /// Interaction logic for TestPage.xaml
  /// </summary>
  public partial class SelectionPage : UserControl
  {
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    CategoryItemsWrapPanelDict categoryItemsWrapPanelDict;
    CategoriesWrapPanel categoriesWrapPanel;
    internal Circle tableUI;
    static App currentApp = (App)Application.Current;
   
    //SolidColorBrush antiqueWhiteBrush = new SolidColorBrush(Colors.AntiqueWhite);

    public SelectionPage()
    {
      InitializeComponent();
      categoryItemsWrapPanelDict = new CategoryItemsWrapPanelDict();
      categoriesWrapPanel = new CategoriesWrapPanel();
      Console.WriteLine("===========Selection Page created");
    }

    private void SelectionPage_Loaded(object sender, RoutedEventArgs e)
    {
      Console.WriteLine("=========SelectionPage_Loaded");
      
      if (tableUI != null)
      {
        TableNumberTextBlock.Text = "Table " + tableUI.Table.TableNumber;
        itemsListView.ItemsSource = tableUI.Table.ItemNameCategoryQuantityList; 
        wrapPanelScrollViewer.Content=categoriesWrapPanel;

        Binding myBinding = new Binding("Table.PriceTotal");
        myBinding.Source = tableUI;
        myBinding.StringFormat = "Total: {0:C2}";
        priceSummary.SetBinding(TextBlock.TextProperty, myBinding);
      }

 
      backToCategoriesButton.Visibility = Visibility.Hidden;
      DisableLeftButtons();

      goHomeButton.Focus();
      itemsListView.SelectedItem = null;
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
        ItemNameCategoryQuantity selectedItem = (ItemNameCategoryQuantity)itemsListView.SelectedItem;
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
        ItemNameCategoryQuantity selectedItem = (ItemNameCategoryQuantity)itemsListView.SelectedItem;
        tableUI.Table.PriceTotal -= selectedItem.ItemsPrice;
        ((ObservableCollection<ItemNameCategoryQuantity>)itemsListView.ItemsSource).Remove(selectedItem);
        
        DisableLeftButtons();
      }
      else
      {
        itemsListView.Focus();
      }   
    }

    private void MinusItemButton_Click(object sender, RoutedEventArgs e)
    {
      ItemNameCategoryQuantity selectedItem = (ItemNameCategoryQuantity)itemsListView.SelectedItem;
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
      ItemNameCategoryQuantity selectedItem = (ItemNameCategoryQuantity)itemsListView.SelectedItem;
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
        foreach (ItemNameCategoryQuantity itemCategoryQuantity in tableUI.Table.ItemNameCategoryQuantityList)
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
        tableUI.Table.ItemNameCategoryQuantityList = new ObservableCollection<ItemNameCategoryQuantity>();
        mainWindow.tabControl.SelectedItem = mainWindow.tablesTab;
        mainWindow.selectionPageTab.IsEnabled = false;
        //update dictionaries
      }
    }

    internal CategoryItemsWrapPanelDict CategoryItemsWrapPanelDict
    {
      get { return this.categoryItemsWrapPanelDict; }
      set { this.categoryItemsWrapPanelDict = value; }
    }

    internal CategoriesWrapPanel CategoriesWrapPanel
    {
      get { return this.categoriesWrapPanel; }
      set { this.categoriesWrapPanel = value; }
    }


  }



  
}
