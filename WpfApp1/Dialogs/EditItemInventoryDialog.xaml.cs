using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WpfApp1.Pages;
using WpfApp1.Dialogs.Templates;
using WpfApp1.Models;

namespace WpfApp1.Dialogs
{
    /// <summary>
    /// Interaction logic for AddInventoryConsumption.xaml
    /// </summary>
    public partial class EditItemInventoryDialog : Window
    {
        List<string> inventoryNameList;
        List<ComboBoxItem> comboBoxItemList;
        public EditItemInventoryDialog()
        {
            InitializeComponent();
            inventoryNameList = new List<string>();
            comboBoxItemList = new List<ComboBoxItem>();


            foreach ( Inventory inventory in ((MainWindow)Application.Current.MainWindow).inventoryPage.inventoryList)
            {
                inventoryNameList.Add(inventory.Name);
            }


        }

        private void AddDependencyButton_Click(object sender, RoutedEventArgs e)
        {
            

            DependencyRow dependencyRow = new DependencyRow();
            dependencyRow.inventoryComboBox.ItemsSource = inventoryNameList;

            dependenciesStackpanel.Children.Add(dependencyRow);
        }
    }
}
