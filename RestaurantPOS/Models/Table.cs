using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RestaurantPOS.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RestaurantPOS.Models
{
  public class Table: INotifyPropertyChanged
  {
    private double priceTotal;
    ObservableCollection<TableItemInfo> tableItemInfosList;

    public Table()
    {
      this.tableItemInfosList = new ObservableCollection<TableItemInfo>();
    }

    public double PriceTotal {
      get { return this.priceTotal; }
      set
      {
        if (value != this.priceTotal)
        {
          this.priceTotal = value;
          NotifyPropertyChanged();
        }
      }
    }

    public Point UpperLeftPoint { get; set; }

    public int TableNumber { get; set; }

    public Boolean IsActive { get; set; }

    public ObservableCollection<TableItemInfo> TableItemInfosList {
      get { return this.tableItemInfosList; }
      set { this.tableItemInfosList = value; }
    }

    internal void UpdateItemPriceInTableItemInfos(string oldName, string oldCategory, double oldPrice, double newPrice)
    {
      foreach (TableItemInfo tableItemInfo in tableItemInfosList)
      {
        if (tableItemInfo.ItemName.Equals(oldName) && tableItemInfo.ItemCategory.Equals(oldCategory))
        { 
          double unitPriceDifference = newPrice - oldPrice;
          tableItemInfo.ItemPrice = newPrice;
          tableItemInfo.ItemsPrice = newPrice * tableItemInfo.ItemQuantity;
          this.PriceTotal = this.PriceTotal + unitPriceDifference * tableItemInfo.ItemQuantity;
        }
      }
    }

    internal void UpdateItemNameCategoryInTableItemInfos(string oldName, string oldCategory, string newName, string newCategory)
    {
      Console.WriteLine("============Update Here");
      foreach (TableItemInfo tableItemInfo in tableItemInfosList)
      {
        if (tableItemInfo.ItemName.Equals(oldName) && tableItemInfo.ItemCategory.Equals(oldCategory))
        {
          tableItemInfo.ItemName = newName;
          tableItemInfo.ItemCategory = newCategory;
        }
      }
    }

    internal void UpdateCategoryInTableItemInfos(string oldCategory, string newCategory)
    {
      foreach (TableItemInfo tableItemInfo in tableItemInfosList)
      {
        if (tableItemInfo.ItemCategory.Equals(oldCategory))
        {
          tableItemInfo.ItemCategory = newCategory;
        }
      }
    }

    internal void RemoveTableItemInfoFromTable(string oldName, string oldCategory)
    {
      for (int i=0; i< TableItemInfosList.Count; i++)
      {
        if (TableItemInfosList[i].ItemName.Equals(oldName) && TableItemInfosList[i].ItemCategory.Equals(oldCategory))
        {
          PriceTotal -= TableItemInfosList[i].ItemsPrice;
          TableItemInfosList.RemoveAt(i);    
          break;
        }
      }
    }

    internal void RemoveTableItemInfoFromTable(string oldCategory)
    {
      for (int i = 0; i < TableItemInfosList.Count; i++)
      {
        if (TableItemInfosList[i].ItemCategory.Equals(oldCategory))
        {
          PriceTotal -= TableItemInfosList[i].ItemsPrice;
          TableItemInfosList.RemoveAt(i);
          break;
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

  }

  

}
