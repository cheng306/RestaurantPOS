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
    ObservableCollection<DatabaseItemNameCategoryQuantity> databaseItemNameCategoryQuantityList;

    public Table()
    {
      this.databaseItemNameCategoryQuantityList = new ObservableCollection<DatabaseItemNameCategoryQuantity>();
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

    public ObservableCollection<DatabaseItemNameCategoryQuantity> DatabaseItemNameCategoryQuantityList {
      get { return this.databaseItemNameCategoryQuantityList; }
      set { this.databaseItemNameCategoryQuantityList = value; }
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

  public class DatabaseItemNameCategoryQuantity : INotifyPropertyChanged
  {
    internal string itemName;
    internal string itemCategory;
    internal int itemQuantity;
    private double itemPrice;
    internal double itemsPrice;



    public string ItemName {
      get { return this.itemName; }
      set {
        if (value != this.itemName)
        {
          this.itemName = value;
          NotifyPropertyChanged();
        }
      }
    }

    public string ItemCategory {
      get { return this.itemCategory; }
      set
      {
        if (value != this.itemCategory)
        {
          this.itemCategory = value;
          NotifyPropertyChanged();
        }
      }
    }

    public int ItemQuantity {
      get { return this.itemQuantity; }
      set
      {
        if (value != this.itemQuantity)
        {
          this.itemQuantity = value;
          NotifyPropertyChanged();
        }
      }
    }

    public double ItemPrice
    {
      get { return this.itemPrice; }
      set
      {
        if (value != this.itemPrice)
        {
          this.itemPrice = value;
          NotifyPropertyChanged();
        }
      }
    }

    public double ItemsPrice
    {
      get { return this.itemsPrice; }
      set
      {
        if (value != this.itemsPrice)
        {
          this.itemsPrice = value;
          NotifyPropertyChanged();
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
