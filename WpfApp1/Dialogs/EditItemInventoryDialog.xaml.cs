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
using System.Collections.ObjectModel;

namespace WpfApp1.Dialogs
{
    /// <summary>
    /// Interaction logic for AddInventoryConsumption.xaml
    /// </summary>
    public partial class EditItemInventoryDialog : Window
    {
        public List<string> inventoryNameList;

        string currentComboBoxOption;
   

        public EditItemInventoryDialog(Item selectedItem)
        {
            InitializeComponent();

            inventoryNameList = new List<string>();
            itemNameTextBlock.Text = selectedItem.Name;

            foreach ( Inventory inventory in ((MainWindow)Application.Current.MainWindow).inventoryPage.inventoryList)
            {
                inventoryNameList.Add(inventory.Name);
            }

            foreach (InventoryConsumption inventoryConsumption in selectedItem.InventoryConsumptionList)
            {
                ObservableCollection<string> comboBoxInventoryList = new ObservableCollection<string>(inventoryNameList);
                inventoryNameList.Remove(inventoryConsumption.InventoryName);
                DependencyRow dependencyRow = new DependencyRow();
                dependencyRow.inventoryComboBox.ItemsSource = comboBoxInventoryList;
                dependencyRow.inventoryComboBox.SelectedItem = inventoryConsumption.InventoryName;
                dependencyRow.quantityTextBox.Text = inventoryConsumption.ConsumptionQUantity.ToString();

                //add event handler to newly added dependencyRow
                dependencyRow.removeDependencyRowButton.Click += RemoveDependencyRowButton_Click;
                dependencyRow.inventoryComboBox.SelectionChanged += InventoryComboBox_SelectionChanged;
                dependenciesStackpanel.Children.Add(dependencyRow);
            }
        }

        private void AddDependencyButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyRow dependencyRow = new DependencyRow();
            dependencyRow.inventoryComboBox.ItemsSource = inventoryNameList;

            dependencyRow.removeDependencyRowButton.Click += RemoveDependencyRowButton_Click;
            dependencyRow.inventoryComboBox.SelectionChanged += InventoryComboBox_SelectionChanged;
            dependenciesStackpanel.Children.Add(dependencyRow);
        }

        private void InventoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void RemoveDependencyRowButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyRow dependencyRow = (DependencyRow)LogicalTreeHelper.GetParent(LogicalTreeHelper.GetParent(LogicalTreeHelper.GetParent((DependencyObject)sender)));
            //add the removed inventory into options of other combobox
            if (dependencyRow.inventoryComboBox.SelectedItem != null)
            {
                string str = (string)dependencyRow.inventoryComboBox.SelectedItem;
                inventoryNameList.Add((string)dependencyRow.inventoryComboBox.SelectedItem);
                foreach (DependencyRow otherDependencyRow in dependenciesStackpanel.Children)
                {
                    ((ObservableCollection<string>)otherDependencyRow.inventoryComboBox.ItemsSource).Add(str);
                }
                inventoryNameList.Add(str);
            } 
            //finally remove it
            dependenciesStackpanel.Children.Remove(dependencyRow);
        }

        
    }
}
