using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using RestaurantPOS.SerializedData;

namespace RestaurantPOS.Pages
{
  /// <summary>
  /// Interaction logic for EditPage.xaml
  /// </summary>
  public partial class EditPage : UserControl
  {
    ObservableCollection<Item> itemsList;
    ObservableCollection<string> categoriesList;

    InventoryPage inventoryPage = ((MainWindow)Application.Current.MainWindow).inventoryPage;
    SelectionPage itemsSelectionPage = ((MainWindow)Application.Current.MainWindow).itemsSelectionPage;
    static App currentApp = (App)Application.Current;
    TablesList tablesList = currentApp.tablesList;

    public EditPage()
    {
      InitializeComponent();

      itemsList = currentApp.itemsList;
      itemsListView.ItemsSource = itemsList;

      categoriesList = currentApp.categoriesList;
      categoriesListBox.ItemsSource = categoriesList;

      InitializeHeadersTag();
      BuildSortDescriptions();

      Console.WriteLine("=========================editPage start=============");
    }

    private void InitializeHeadersTag()
    {
      nameHeader.Tag = new NameSortDir { Name = "Name", SortDirection = ListSortDirection.Descending };
      categoryHeader.Tag = new NameSortDir { Name = "Category", SortDirection = ListSortDirection.Descending };
      priceHeader.Tag = new NameSortDir { Name = "Price", SortDirection = ListSortDirection.Descending };
      addTimeHeader.Tag = new NameSortDir { Name = "AddTime", SortDirection = ListSortDirection.Ascending };
    }

    private void BuildSortDescriptions()
    {
      itemsListView.Items.SortDescriptions.Add(new SortDescription("AddTime", ListSortDirection.Ascending));
      itemsListView.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
      itemsListView.Items.SortDescriptions.Add(new SortDescription("Category", ListSortDirection.Ascending));
      itemsListView.Items.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Ascending));
    }

    private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
    {
      GridViewColumnHeader header = (GridViewColumnHeader)sender;
      SortDescriptionCollection sortDescriptionCollection = itemsListView.Items.SortDescriptions;
      for (int i = 0; i < sortDescriptionCollection.Count; i++)
      {
        if (((NameSortDir)header.Tag).Name.Equals(sortDescriptionCollection[i].PropertyName))
        {
          SortDescription oldSortDescription = sortDescriptionCollection[i];
          SortDescription newSortDescription;
          for (int j = i; j > 0; j--)
          {
            sortDescriptionCollection[j] = sortDescriptionCollection[j - 1];
          }

          if (oldSortDescription.Direction == ListSortDirection.Ascending)
          {
            newSortDescription = new SortDescription(oldSortDescription.PropertyName, ListSortDirection.Descending);
          }
          else
          {
            newSortDescription = new SortDescription(oldSortDescription.PropertyName, ListSortDirection.Ascending);
          }

          sortDescriptionCollection[0] = newSortDescription;

          break;
        }
      }
    }

    private void AddItemButton_Click(object sender, RoutedEventArgs e)
    {
      EditItemDialog addItemWindow = new EditItemDialog();
      Console.WriteLine("id: "+addItemWindow.GetHashCode());
      if (addItemWindow.ShowDialog() == true)
      {
        Item newItem = new Item
        {
          Name = addItemWindow.ItemName,
          Category = addItemWindow.ItemCategory,
          Price = addItemWindow.ItemPrice,
          AddTime = DateTime.Now
        };
        itemsList.Add(newItem);

        //modify itemsSelectionPage
        itemsSelectionPage.CategoryItemsWrapPanelDict.AddItemToItemsWrapPanel(newItem);

        //update categoryItemDict
        currentApp.AddItemToCategoryItemDict(addItemWindow.ItemCategory, newItem);

        //update ItemNameObjectDict
        currentApp.ItemNameObjectDict.AddItemToItemNameObjectDict(newItem);

        //update the view of Edit Page
        itemsListView.SelectedItem = newItem;
        itemsListView.Focus();
      }
      else
      {
        if (itemsListView.SelectedItem != null)
        {
          itemsListView.Focus();
          EnableModifyAndDeleteItemButton();
        }
      }
      addItemWindow.DisableDialog();
      //addItemWindow.Close();
    }

    private void ModifyItemButton_Click(object sender, RoutedEventArgs e)
    {
      Item selectedItem = (Item)itemsListView.SelectedItem;
      if (selectedItem != null)
      {
        EditItemDialog editItemDialog = new EditItemDialog(selectedItem);
        Console.WriteLine("id: " + editItemDialog.GetHashCode());
        if (editItemDialog.ShowDialog() == true)
        {
          string oldName = selectedItem.Name;
          string oldCategory = selectedItem.Category;
          double oldPrice = selectedItem.Price;

          string newName = editItemDialog.ItemName;
          string newCategory = editItemDialog.ItemCategory;
          double newPrice = editItemDialog.ItemPrice;

          //Update tables' ItemNameCategoryQunatityList
          tablesList.UpdateItemNameCategoryQunatityListInTables(oldName, oldCategory, newName, newCategory);
          
          if (!newCategory.Equals(oldCategory))
          {
            //update CategoryItemDict
            currentApp.ChangeItemCategoryInCategoryItemDict(selectedItem, oldCategory, editItemDialog.ItemCategory);
          }

          if (!newName.Equals(oldName))
          {
            //update ItemNameObjectDict
            currentApp.ItemNameObjectDict.EditItemNameInItemNameObjectDict(oldName, newName);
          }
          selectedItem.Name = newName;
          selectedItem.Category = newCategory;
          selectedItem.Price = newPrice;
          
        }
        editItemDialog.DisableDialog();

      }
    }

    private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
    {
      //Get the selected Items List
      IList selectedItemsList = itemsListView.SelectedItems;
      int selectedItemsCount = selectedItemsList.Count;
      string removeItemsMessage = "Do you want to delete " + selectedItemsCount + " items";

      MessageBoxResult mResult = MessageBox.Show(removeItemsMessage, "Delete Items", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.No);
      if (mResult == MessageBoxResult.Yes)
      {
        Item[] removeItemsArray = new Item[selectedItemsCount];
        for (int i = 0; i < selectedItemsCount; i++)
        {
          removeItemsArray[i] = (Item)selectedItemsList[i];
        }

        for (int i = 0; i < removeItemsArray.Length; i++)
        {
          //update tables' temNameCategoryQunatity
          tablesList.RemoveItemNameCategoryQunatityFromTables(removeItemsArray[i].Name, removeItemsArray[i].Category);
          itemsList.Remove(removeItemsArray[i]);

          //update itemsSelectionPage
          itemsSelectionPage.CategoryItemsWrapPanelDict.RemoveItemFromItemsWrapPanel(removeItemsArray[i]);

          //update categoryItemDict
          currentApp.RemoveItemFromCategoryItemDict(removeItemsArray[i]);

          //update itemNameObjectDict
          currentApp.ItemNameObjectDict.RemoveItemFromItemNameObjectDict(removeItemsArray[i]);

          //update inventoryItemsDict
          foreach (InventoryConsumption inventoryConsumption in removeItemsArray[i].InventoryConsumptionList)
          {
            currentApp.InventoryNameItemsListDict[inventoryConsumption.InventoryName].Remove(removeItemsArray[i]);
          }
        }
      }

      DisableModifyAndDeleteItemButton();
    }

    private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
    {
      EditCategoryDialog addCategoryWindow = new EditCategoryDialog();
      if (addCategoryWindow.ShowDialog() == true)
      {
        categoriesList.Add(addCategoryWindow.Input);
        categoriesListBox.SelectedItem = addCategoryWindow.Input;

        //update itemsSelectionPage
        itemsSelectionPage.CategoryItemsWrapPanelDict.AddCategoryToCategoryItemsWrapPanelDict(addCategoryWindow.Input);
        itemsSelectionPage.CategoriesWrapPanel.AddCategoryToCategoriesWrapPanel(addCategoryWindow.Input);

        //modify categoryItemDict
        currentApp.AddCategoryToCategoryItemDict(addCategoryWindow.Input);

        categoriesListBox.SelectedItem = addCategoryWindow.Input;
      }
      else
      {
        if (categoriesListBox.SelectedItem != null)
        {
          EnableModifyAndDeleteCategoryButton();
        }
      }      
      categoriesListBox.Focus();
    }

    private void ModifyCategoryButton_Click(object sender, RoutedEventArgs e)
    {
      int selectedIndex; 

      if (categoriesListBox.SelectedItem != null)
      {
        //undate the category list 
        selectedIndex = categoriesListBox.SelectedIndex;
        EditCategoryDialog editCategoryDialog = new EditCategoryDialog((string)categoriesListBox.SelectedItem);

        if (editCategoryDialog.ShowDialog() == true)
        {
          string oldCategory = (string)categoriesListBox.SelectedItem;
          string newCategory = editCategoryDialog.Input;
          if (!oldCategory.Equals(newCategory))
          {
            //update categoriesList
            ((ObservableCollection<string>)categoriesListBox.ItemsSource)[selectedIndex] = editCategoryDialog.Input;
            
            //update categoryItemDict
            currentApp.ModifyCategoryInCategoryItemDict(oldCategory, editCategoryDialog.Input);
            UpdateItemCategoryProperty(editCategoryDialog.Input);
            
            //update SelectionPage
            itemsSelectionPage.CategoryItemsWrapPanelDict.ModifyCategoryInCategoryItemsWrapPanelDict(oldCategory, editCategoryDialog.Input);
            itemsSelectionPage.CategoriesWrapPanel.ModifyCategoryInCategoryWrapPanel(oldCategory, editCategoryDialog.Input);

            //update 
            tablesList.UpdateCategoryInItemNameCategoryQuantityLists(oldCategory, newCategory);
          }
        }
        categoriesListBox.SelectedIndex = selectedIndex;
      }

      //after the dialog close  
      categoriesListBox.Focus();
    }


    private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
    {
      //Get the selected Items List
      IList selectedCategoriesList = categoriesListBox.SelectedItems;
      int selectedCategoriesCount = selectedCategoriesList.Count;
      string removeCategoriesMessage = "Do you want to delete " + selectedCategoriesCount + " Category and ALL assocaited items";

      YesNoCancelDialog yesNoCancelDialog = new YesNoCancelDialog(removeCategoriesMessage);
      if (yesNoCancelDialog.ShowDialog() == true)
      {
        string[] removeCategoriesArray = new string[selectedCategoriesCount];
        for (int i = 0; i < selectedCategoriesCount; i++)
        {
          removeCategoriesArray[i] = (string)selectedCategoriesList[i];
        }

        for (int i = 0; i < removeCategoriesArray.Length; i++)
        {
          categoriesList.Remove(removeCategoriesArray[i]);

          //udate itemsSelectionPage
          itemsSelectionPage.CategoriesWrapPanel.RemoveCategoryFromCategoriesWrapPanel(removeCategoriesArray[i]);
          itemsSelectionPage.CategoryItemsWrapPanelDict.RemoveCategoryFromCategoryItemsWrapPanelDict(removeCategoriesArray[i]);

          //remove all items of removed category
          RemoveItemsOfCategory(removeCategoriesArray[i]);
          
          //update categoryItemDict
          currentApp.RemoveCategoryFromCategoryItemDict(removeCategoriesArray[i]);
        }
      }

      DisableModifyAndDeleteCategoryButton();
    }

    private void UpdateItemCategoryProperty(string category)
    {
      List<Item> selectedItemsList = currentApp.CategoryItemsDict[category];
      foreach (Item item in selectedItemsList)
      {
        item.Category = category;
      }
    }

    private void RemoveItemsOfCategory(string category)
    {
      List<Item> selectedItemsList = currentApp.CategoryItemsDict[category];
      foreach (Item item in selectedItemsList)
      {
        itemsList.Remove(item);
      }
    }

    //below are about focus and buttons ability
    private void EditPage_Loaded(object sender, RoutedEventArgs e)
    {
      itemsListView.SelectedItem = null;
      categoriesListBox.SelectedItem = null;
      DisableModifyAndDeleteItemButton();
      DisableModifyAndDeleteCategoryButton();
      
      Console.WriteLine("==============editPage Loaded");
    }

    private void LeftGrid_GotFocus(object sender, RoutedEventArgs e)
    {
      DisableModifyAndDeleteCategoryButton();
    }

    private void RightGrid_GotFocus(object sender, RoutedEventArgs e)
    {
      DisableModifyAndDeleteItemButton();
    }

    private void ItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      EnableModifyAndDeleteItemButton();
    }

    private void ItemsListView_GotFocus(object sender, RoutedEventArgs e)
    {
      if (itemsListView.SelectedItem != null)
      {
        EnableModifyAndDeleteItemButton();
      }
    }

    private void CategoriesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      EnableModifyAndDeleteCategoryButton();
    }

    private void CategoriesListBox_GotFocus(object sender, RoutedEventArgs e)
    {
      if (categoriesListBox.SelectedItem != null)
      {
        EnableModifyAndDeleteCategoryButton();
      }
    }

    private void DisableModifyAndDeleteCategoryButton()
    {
      modifyCategoryButton.IsEnabled = false;
      deleteCategoryButton.IsEnabled = false;
    }

    private void EnableModifyAndDeleteCategoryButton()
    {
      modifyCategoryButton.IsEnabled = true;
      deleteCategoryButton.IsEnabled = true;
    }

    private void DisableModifyAndDeleteItemButton()
    {
      modifyItemButton.IsEnabled = false;
      deleteItemButton.IsEnabled = false;
    }

    private void EnableModifyAndDeleteItemButton()
    {
      modifyItemButton.IsEnabled = true;
      deleteItemButton.IsEnabled = true;
    }
    //above are about focus and buttons ability

   

    
  }


  //NameSortDir class is for Tag properrty of GridViewColumnHeader
  internal class NameSortDir
  {
    internal string Name { get; set; }
    internal ListSortDirection SortDirection { get; set; }
  }
}
