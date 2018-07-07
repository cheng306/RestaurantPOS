using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantPOS.Models
{
  public class Item : INotifyPropertyChanged
  {
    private List<InventoryConsumption> inventoryConsumptionList;
    private string category;
    private string name;
    private double price;

    public Item()
    {
      inventoryConsumptionList = new List<InventoryConsumption>();
    }

    public int Id { get; set; }

    public string Name {
      get { return this.name; }
      set
      {
        if (value!= this.name)
        {
          this.name = value;
          NotifyPropertyChanged();
        }
      }
    }

    public string Category
    {
      get { return this.category; }
      set
      {
        if (value != this.category)
        {
          this.category = value;
          NotifyPropertyChanged();
        }
      }
    }

    public double Price {
      get { return this.price; }
      set
      {
        if (value != this.price)
        {
          this.price = value;
          NotifyPropertyChanged();
     
        }
      }
    }

    public DateTime AddTime { get; set; }

    public List<InventoryConsumption> InventoryConsumptionList
    {
      get { return this.inventoryConsumptionList; }
      set
      {
        if (value != this.inventoryConsumptionList)
        {
          this.inventoryConsumptionList = value;
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
