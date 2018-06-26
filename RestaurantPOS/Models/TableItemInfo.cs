using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantPOS.Models
{
  public class TableItemInfo : INotifyPropertyChanged
  {
    internal string itemName;
    internal string itemCategory;
    internal int itemQuantity;
    private double itemPrice;
    internal double itemsPrice;

    public string ItemName
    {
      get { return this.itemName; }
      set
      {
        if (value != this.itemName)
        {
          this.itemName = value;
          NotifyPropertyChanged();
        }
      }
    }

    public string ItemCategory
    {
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

    public int ItemQuantity
    {
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
