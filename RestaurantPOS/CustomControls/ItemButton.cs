using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using RestaurantPOS.Models;

namespace RestaurantPOS.CustomControls
{
  public class ItemButton : Button
  {
    static App currentApp = (App)Application.Current;
    static PropertyPath propertyPath = new PropertyPath("Name");
    MainWindow mainWindow = (MainWindow)currentApp.MainWindow;
    Binding itemBinding;
    SolidColorBrush antiqueWhiteBrush = currentApp.antiqueWhiteBrush;

    public ItemButton(Item item)
    {
      //Button binding
      itemBinding = new Binding();
      itemBinding.Source = item;
      itemBinding.Mode = BindingMode.OneWay;
      itemBinding.Path = propertyPath;
      itemBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
      BindingOperations.SetBinding(this, ContentProperty, itemBinding);

      //Button Apperance
      this.Margin = new Thickness(10);
      this.Width = 150;
      this.Height = 100;
      this.Background = antiqueWhiteBrush;

      //add listener to the Button
      this.Click += ItemButton_Click;
    }

    private void ItemButton_Click(object sender, RoutedEventArgs e)
    {
      ItemButton itemButton = (ItemButton)sender;
      ObservableCollection<TableItemInfo> tableItemInfosList =
        (ObservableCollection<TableItemInfo>)mainWindow.itemsSelectionPage.itemsListView.ItemsSource;

      bool newItemInTable = true;
      for (int i = 0; i < tableItemInfosList.Count; i++)
      {
        if (tableItemInfosList[i].ItemName.Equals(itemButton.Content.ToString()) &&
          tableItemInfosList[i].ItemCategory.Equals(itemButton.ButtonItem.Category) &&
          tableItemInfosList[i].ItemPrice == itemButton.ButtonItem.Price)
        {
          newItemInTable = false;
          tableItemInfosList[i].ItemQuantity++;
          tableItemInfosList[i].ItemsPrice += itemButton.ButtonItem.Price;
          mainWindow.itemsSelectionPage.tableUI.Table.PriceTotal += itemButton.ButtonItem.Price;
          break;
        }
      }
      if (newItemInTable)
      {
        tableItemInfosList.
          Add(new TableItemInfo
        {
          ItemName = itemButton.Content.ToString(),
          ItemCategory = itemButton.ButtonItem.Category,
          ItemQuantity = 1,
          ItemPrice = itemButton.ButtonItem.Price,
          ItemsPrice = itemButton.ButtonItem.Price
        });
        mainWindow.itemsSelectionPage.tableUI.Table.PriceTotal += itemButton.ButtonItem.Price;
      }
    }

    internal Item ButtonItem
    {
      get {return (Item)this.itemBinding.Source; }     
    }
   

  } 
}
