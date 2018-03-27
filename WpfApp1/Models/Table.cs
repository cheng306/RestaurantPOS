using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RestaurantPOS.Pages;
using System.Collections.ObjectModel;

namespace RestaurantPOS.Models
{
  public class Table
  {
    ObservableCollection<ItemNameQuantity> itemNameQuantityList;

    public Table()
    {

    }

    public Table(Point point)
    {
      this.UpperLeftPoint = point;
    }

    public Point UpperLeftPoint { get; set; }

    public int TableNumber { get; set; }

    public Boolean IsActive { get; set; }

    public ObservableCollection<ItemNameQuantity> ItemNameQuantityList { get; set; }

  }

  public class ItemNameQuantity
  {
    public string ItemName { get; set; }

    public int ItemQuantity { get; set; }
  }

}
