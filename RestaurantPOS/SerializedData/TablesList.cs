using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantPOS.Models;

namespace RestaurantPOS.SerializedData
{
  public class TablesList : List<Table>
  {
    // Update item
    internal void UpdateItemNameCategoryQunatityListInTables(string oldName, string oldCategory, string newName, string newCategory)
    {
      if (!oldName.Equals(newName) || !oldCategory.Equals(newCategory))
      {
        foreach (Table table in this)
        {
          if (table.IsActive)
          {
            table.UpdateItemNameCategoryQuantity(oldName, oldCategory, newName, newCategory);
          }
        }
      }
    }

    internal void UpdateCategoryInItemNameCategoryQuantityLists(string oldCategory, string newCategory)
    {
      if (!oldCategory.Equals(newCategory))
      {
        foreach (Table table in this)
        {
          if (table.IsActive)
          {
            table.UpdateCategoryInItemNameCategoryQuantityList(oldCategory, newCategory);
          }
        }
      }
    }

    internal void RemoveItemNameCategoryQunatityFromTables(string oldName, string oldCategory)
    {
      foreach (Table table in this)
      {
        if (table.IsActive)
        {
          for (int i=0; i<table.ItemNameCategoryQuantityList.Count;i++)
          {
            if (table.ItemNameCategoryQuantityList[i].ItemName.Equals(oldName) && table.ItemNameCategoryQuantityList[i].ItemCategory.Equals(oldCategory))
            {
              table.ItemNameCategoryQuantityList.RemoveAt(i);
              break;
            }
          }   
        }
      }
    }


  }
}
