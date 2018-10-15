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
        itemsListView.ItemsSource = tableUI.Table.TableItemInfosList; 
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
        TableItemInfo selectedItem = (TableItemInfo)itemsListView.SelectedItem;
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
        TableItemInfo selectedItem = (TableItemInfo)itemsListView.SelectedItem;
        tableUI.Table.PriceTotal -= selectedItem.ItemsPrice;
        ((ObservableCollection<TableItemInfo>)itemsListView.ItemsSource).Remove(selectedItem);
        
        DisableLeftButtons();
      }
      else
      {
        itemsListView.Focus();
      }   
    }

    private void MinusItemButton_Click(object sender, RoutedEventArgs e)
    {
      TableItemInfo selectedItem = (TableItemInfo)itemsListView.SelectedItem;
      tableUI.Table.PriceTotal -= selectedItem.ItemPrice;
      selectedItem.ItemsPrice -= selectedItem.ItemPrice;  
      selectedItem.ItemQuantity--;

      if (selectedItem.ItemQuantity == 1)
      {
        minusItemButton.IsEnabled = false;
      }
      itemsListView.Focus();
      
    }

    private void AddItemButton_Click(object sender, RoutedEventArgs e)
    {
      TableItemInfo selectedItem = (TableItemInfo)itemsListView.SelectedItem;
      tableUI.Table.PriceTotal += selectedItem.ItemPrice;
      selectedItem.ItemsPrice += selectedItem.ItemPrice;
      
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
        foreach (TableItemInfo itemCategoryQuantity in tableUI.Table.TableItemInfosList)
        {
          if (itemNameObjectDict.ContainsKey(itemCategoryQuantity.ItemName))
          {
            Item consumedItem = itemNameObjectDict[itemCategoryQuantity.ItemName];
            foreach (InventoryConsumption inventoryConsumption in consumedItem.InventoryConsumptionList)
            {
              if (inventoryNameObjectDict.ContainsKey(inventoryConsumption.InventoryName))
              {
                Inventory inventory = inventoryNameObjectDict[inventoryConsumption.InventoryName];
                inventory.Quantity -= (inventoryConsumption.ConsumptionQuantity * itemCategoryQuantity.ItemQuantity); 
                if (inventory.Quantity <= inventory.RemindingLevel)
                {
                  MessageBoxDialog messageBox = new MessageBoxDialog(inventory.Name + " is running Low");
                  messageBox.Show();
                  //MessageBox.Show(inventory.Name + " is running Low", "Low Inventory Level", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
              }
            }
          }       
        }
        tableUI.Table.TableItemInfosList = new ObservableCollection<TableItemInfo>();
        tableUI = null; 
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
