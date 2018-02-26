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
using System.Collections;


namespace WpfApp1.Dialogs
{
    /// <summary>
    /// Interaction logic for AddInventoryConsumption.xaml
    /// </summary>
    public partial class EditItemInventoryDialog : Window
    {
        public List<string> inventoryNameList;
        
        public EditItemInventoryDialog(Item selectedItem)
        {
            InitializeComponent();

            inventoryNameList = new List<string>();
            itemNameTextBlock.Text = selectedItem.Name;

            foreach ( Inventory inventory in ((MainWindow)Application.Current.MainWindow).inventoryPage.inventoryList)
            {
                inventoryNameList.Add(inventory.Name);
            }

            //for existing inventoryConsumption, create depedencyRow for each consumption
            foreach (InventoryConsumption inventoryConsumption in selectedItem.InventoryConsumptionList)
            {
                ObservableCollection<string> comboBoxInventoryList = new ObservableCollection<string>(inventoryNameList);
                inventoryNameList.Remove(inventoryConsumption.InventoryName);
                DependencyRow dependencyRow = new DependencyRow();
                dependencyRow.inventoryComboBox.ItemsSource = comboBoxInventoryList;
                dependencyRow.inventoryComboBox.SelectedItem = inventoryConsumption.InventoryName;
                dependencyRow.quantityTextBox.Text = inventoryConsumption.ConsumptionQuantity.ToString();

                //add event handler to newly added dependencyRow
                dependencyRow.removeDependencyRowButton.Click += RemoveDependencyRowButton_Click;
                dependencyRow.inventoryComboBox.SelectionChanged += InventoryComboBox_SelectionChanged;
                dependencyRow.quantityTextBox.TextChanged += QuantityTextBox_TextChanged;
                dependenciesStackpanel.Children.Add(dependencyRow);
            }
        }

        private void AddDependencyButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyRow dependencyRow = new DependencyRow();
            ObservableCollection<string> comboBoxInventoryList = new ObservableCollection<string>(inventoryNameList);
            dependencyRow.inventoryComboBox.ItemsSource = comboBoxInventoryList;

            //add event handler to newly added dependencyRow
            dependencyRow.removeDependencyRowButton.Click += RemoveDependencyRowButton_Click;
            dependencyRow.inventoryComboBox.SelectionChanged += InventoryComboBox_SelectionChanged;
            dependencyRow.quantityTextBox.TextChanged += QuantityTextBox_TextChanged;
            dependenciesStackpanel.Children.Add(dependencyRow);

            ValidationCheck();
        }

        private void RemoveDependencyRowButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyRow dependencyRow = (DependencyRow)LogicalTreeHelper.GetParent(LogicalTreeHelper.GetParent(LogicalTreeHelper.GetParent((DependencyObject)sender)));
            //add the removed inventory into options of other combobox
            if (dependencyRow.inventoryComboBox.SelectedItem != null)
            {
                string selectedOption = (string)dependencyRow.inventoryComboBox.SelectedItem;
                AddOptionsToAllComboBox(selectedOption);
            }
            //finally remove it
            dependenciesStackpanel.Children.Remove(dependencyRow);

            ValidationCheck();
        }

        private void InventoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IList removedInventoryList =  e.RemovedItems;
            ComboBox comboBox = (ComboBox)sender; 
            
            if (removedInventoryList.Count != 0)
            {
                string previousComboBoxOption = (string)removedInventoryList[0];
                AddOptionsToAllComboBox(previousComboBoxOption);   
            }

            RemoveOptionsfromOtherComboBox(comboBox);

            ValidationCheck();
        }

        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidationCheck();
        }



        private void AddOptionsToAllComboBox(string option)
        {
            foreach (DependencyRow otherDependencyRow in dependenciesStackpanel.Children)
            {
                ((ObservableCollection<string>)otherDependencyRow.inventoryComboBox.ItemsSource).Add(option);
            }
            inventoryNameList.Add(option);
        }

        private void RemoveOptionsfromOtherComboBox(ComboBox comboBox)
        {
            foreach (DependencyRow otherDependencyRow in dependenciesStackpanel.Children)
            {
                if (otherDependencyRow.inventoryComboBox!= comboBox)
                {
                    ((ObservableCollection<String>)otherDependencyRow.inventoryComboBox.ItemsSource).Remove((string)comboBox.SelectedItem);
                    Console.WriteLine("======remove option");
                }     
            }
            inventoryNameList.Remove((string)comboBox.SelectedItem);
            
        }

       
        private void ValidationCheck()
        {
            bool flag = true;
            foreach (DependencyRow dependencyRow in dependenciesStackpanel.Children)
            {
                if (!dependencyRow.ValidDependencyRow)
                {
                    flag = false;
                    Console.WriteLine("==========not okay");
                    break;
                }
            }
            if (flag)
            {
                confrimButton.IsEnabled = true;
            }
            else{
                confrimButton.IsEnabled = false;
            }
        }

        private void ConfrimButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
