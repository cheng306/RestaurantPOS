using System;
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

        internal List<Item> itemList;
        internal ObservableCollection<Item> itemList2;
        public EditPage()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            itemList.Add(new Item { Name = "sdf", Category = "sdf", Price = 12.2 });
        }

        public List<Item> ItemList
        {
            get
            {
                return itemList;
            }
            set
            {
                this.itemList = value;
            }
        }

        
    }

    
}
