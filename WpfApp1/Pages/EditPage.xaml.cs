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
using WpfApp1.Models;
using WpfApp1.Windows;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Interaction logic for EditPage.xaml
    /// </summary>
    public partial class EditPage : UserControl
    {

        //internal List<Item> itemList;
        internal ObservableCollection<Item> itemList;
        internal ObservableCollection<string> categoriesList;

        public EditPage()
        {
            InitializeComponent();

            Console.WriteLine("=========================in editpage=============");
   
            InitializeHeadersTag();

            BuildSortDescriptions();
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
            for (int i=0; i< sortDescriptionCollection.Count; i++)
            {
                if (((NameSortDir)header.Tag).Name.Equals(sortDescriptionCollection[i].PropertyName))
                {
                    SortDescription oldSortDescription = sortDescriptionCollection[i];
                    SortDescription newSortDescription; 
                    for (int j =i; j>0; j--)
                    {
                        sortDescriptionCollection[j] = sortDescriptionCollection[j - 1];
                    }

                    if (oldSortDescription.Direction == ListSortDirection.Ascending)
                    {
                        newSortDescription = new SortDescription(oldSortDescription.PropertyName,ListSortDirection.Descending);
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
            AddItemWindow addItmWindow = new AddItemWindow();
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow addCategoryWindow = new AddCategoryWindow();
            if (addCategoryWindow.ShowDialog() == true)
            {
                categoriesList.Add(addCategoryWindow.Input);
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
                    itemList.Remove(removeItemsArray[i]);
                }
            }
        }

        private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            //Get the selected Items List
            IList selectedCategoriesList = categoriesListBox.SelectedItems;
            int selectedCategoriesCount = selectedCategoriesList.Count;
            string removeCategoriesMessage = "Do you want to delete " + selectedCategoriesCount + " items";

            MessageBoxResult mResult = MessageBox.Show(removeCategoriesMessage, "Delete Categories", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.No);
            if (mResult == MessageBoxResult.Yes)
            {
                string[] removeCategoriesArray = new string[selectedCategoriesCount];
                for (int i = 0; i < selectedCategoriesCount; i++)
                {
                    removeCategoriesArray[i] = (string)selectedCategoriesList[i];
                }

                for (int i = 0; i < removeCategoriesArray.Length; i++)
                {
                    categoriesList.Remove(removeCategoriesArray[i]);
                }
            }
        }

        private void ItemsListView_GotFocus(object sender, RoutedEventArgs e)
        {
            deleteItemButton.IsEnabled = true;
            deleteCategoryButton.IsEnabled = false;
            Console.WriteLine("ItemsListView gain focus");
        }

        private void CategoriesListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            deleteCategoryButton.IsEnabled = true;
            deleteItemButton.IsEnabled = false; 
            Console.WriteLine("CategoriesListBox gain focus");
        }

        private void EditPage_Loaded(object sender, RoutedEventArgs e)
        {
            deleteCategoryButton.IsEnabled = false;
            deleteItemButton.IsEnabled = false;
            Console.WriteLine("editPage Loaded");
        }

        
    }


    //NameSortDir class is for Tag properrty of GridViewColumnHeader
    internal class NameSortDir
    {
        internal string Name { get; set; }
        internal ListSortDirection SortDirection { get; set; }
    }

    
}
