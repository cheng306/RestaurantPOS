﻿using System;
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
    internal void UpdateItemNameCategoryPriceInTableItemInfos(string oldName, string oldCategory, double oldPrice, string newName, string newCategory, double newPrice)
    {
      if (oldPrice!= newPrice)
      {
        foreach (Table table in this)
        {
          if (table.IsActive)
          {
            table.UpdateItemPriceInTableItemInfos(oldName, oldCategory, oldPrice, newPrice);
          }
        }
      }
      if (!oldName.Equals(newName) || !oldCategory.Equals(newCategory))
      {
        foreach (Table table in this)
        {
          if (table.IsActive)
          {
            table.UpdateItemNameCategoryInTableItemInfos(oldName, oldCategory, newName, newCategory);
          }
        }
      }
    }

    internal void UpdateCategoryInTableItemInfos(string oldCategory, string newCategory)
    {
      if (!oldCategory.Equals(newCategory))
      {
        foreach (Table table in this)
        {
          if (table.IsActive)
          {
            table.UpdateCategoryInTableItemInfos(oldCategory, newCategory);
          }
        }
      }
    }

    internal void RemoveTableItemInfoFromTables(string oldName, string oldCategory)
    {
      foreach (Table table in this)
      {
        if (table.IsActive)
        {
          table.RemoveTableItemInfoFromTable(oldName, oldCategory);
        }
      }
    }

    internal void RemoveTableItemInfoFromTables(string oldCategory)
    {
      foreach (Table table in this)
      {
        if (table.IsActive)
        {
          table.RemoveTableItemInfoFromTable(oldCategory);
        }
      }
    }




  }
}
