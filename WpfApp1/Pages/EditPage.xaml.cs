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

namespace WpfApp1.Pages
{
    /// <summary>
    /// Interaction logic for EditPage.xaml
    /// </summary>
    public partial class EditPage : UserControl
    {

        //internal List<Item> itemList;
        internal ObservableCollection<Item> itemList;
        public EditPage()
        {
            InitializeComponent();

            Console.WriteLine("=========================in editpage=============");

            InitializeHeadersTag();
            BuildSortDescriptions();


            
            //itemsListView.Items.SortDescriptions.Clear();



        }

        

        private void DeleteButton_Click(object sender, RoutedEventArgs e)   
        {
            IList removeItemsList = itemsListView.SelectedItems;
            Item[] removeItemsArray = new Item[removeItemsList.Count];

            for (int i =0; i< removeItemsList.Count; i++)
            {
                removeItemsArray[i] = (Item)removeItemsList[i];
            }

            for (int i=0; i< removeItemsArray.Length; i++)
            {
                itemList.Remove(removeItemsArray[i]);
            }
 
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader header = (GridViewColumnHeader)sender;

            //itemsListView.Items.SortDescriptions.Add( new SortDescription("Content", ListSortDirection.Descending));
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
    }

    internal class NameSortDir
    {
        internal string Name { get; set; }
        internal ListSortDirection SortDirection { get; set; }
    }

    
}
