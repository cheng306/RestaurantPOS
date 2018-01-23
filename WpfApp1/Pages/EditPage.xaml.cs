using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            //tb1.DataContext = Application.Current.MainWindow;
            Console.WriteLine("=========================in editpage=============");


            Console.WriteLine(grid.ActualWidth);

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)   
        {

            IList removeItemsList = itemListView.SelectedItems;
            Item[] removeItemsArray = new Item[removeItemsList.Count];

            for (int i =0; i< removeItemsList.Count; i++)
            {
                removeItemsArray[i] = (Item)removeItemsList[i];
            }

            for (int i=0; i< removeItemsArray.Length; i++)
            {
                itemList.Remove(removeItemsArray[i]);
            }



            //foreach (Item i in ilist)
            //{
            //    itemList.Remove(i);
            //}
        }
    }

    
}
