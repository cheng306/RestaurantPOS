using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RestaurantPOS.Models
{
  public class ItemButton : Button
  {
    static PropertyPath propertyPath = new PropertyPath("Name");
    Binding itemBinding;

    public ItemButton(Item item)
    {
      itemBinding = new Binding();
      itemBinding.Source = item;
      itemBinding.Mode = BindingMode.OneWay;
      itemBinding.Path = propertyPath;
      itemBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
      BindingOperations.SetBinding(this, ContentProperty, itemBinding);
    }

    internal Item ButtonItem
    {
      get {return (Item)this.itemBinding.Source; }
      
    }
   

  }
}
