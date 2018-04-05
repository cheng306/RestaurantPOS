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
using RestaurantPOS.Pages;
using RestaurantPOS.Dialogs.Templates;
using RestaurantPOS.Models;
using System.Collections.ObjectModel;
using System.Collections;


namespace RestaurantPOS.Dialogs
{
  /// <summary>
  /// Interaction logic for AddInventoryConsumption.xaml
  /// </summary>
  public partial class EditItemInventoryDialog : Window
  {
    public List<string> inventoryNameList;
    public ObservableCollection<Inventory> inventoryList;

    public EditItemInventoryDialog(Item selectedItem)
    {
      InitializeComponent();

      inventoryNameList = new List<string>();
      itemNameTextBlock.Text = selectedItem.Name;

      inventoryList = ((MainWindow)Application.Current.MainWindow).inventoryPage.inventoryList;
      foreach (Inventory inventory in inventoryList)
      {
        inventoryNameList.Add(inventory.Name);
      }

      //for existing inventoryConsumption, create depedencyRow for each consumption
      if (selectedItem.InventoryConsumptionList != null)
      {
        foreach (InventoryConsumption inventoryConsumption in selectedItem.InventoryConsumptionList)
        {
          ObservableCollection<string> comboBoxInventoryList = new ObservableCollection<string>(inventoryNameList);

          DependencyRow dependencyRow = new DependencyRow();

          dependencyRow.inventoryComboBox.ItemsSource = comboBoxInventoryList;
          dependencyRow.inventoryComboBox.SelectedItem = inventoryConsumption.InventoryName;

          inventoryNameList.Remove(inventoryConsumption.InventoryName);
          RemoveOptionsfromOtherComboBox(dependencyRow.inventoryComboBox);

          dependencyRow.quantityTextBox.Text = inventoryConsumption.ConsumptionQuantity.ToString();

          foreach (Inventory inventory in inventoryList)
          {
            if (inventory.Name.Equals(inventoryConsumption.InventoryName))
            {
              dependencyRow.unitTextBlock.Text = inventory.Unit;
              break;
            }
          }

          //add event handler to newly added dependencyRow
          AddEventHandlersToDependencyRow(dependencyRow);

          dependenciesStackpanel.Children.Add(dependencyRow);
        }
      }

    }

    private void AddDependencyButton_Click(object sender, RoutedEventArgs e)
    {
      DependencyRow dependencyRow = new DependencyRow();
      ObservableCollection<string> comboBoxInventoryList = new ObservableCollection<string>(inventoryNameList);
      dependencyRow.inventoryComboBox.ItemsSource = comboBoxInventoryList;

      //add event handler to newly added dependencyRow
      AddEventHandlersToDependencyRow(dependencyRow);

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
        AddOptionsToOtherComboBox(selectedOption, null);
      }
      //finally remove it
      dependenciesStackpanel.Children.Remove(dependencyRow);

      ValidationCheck();
    }

    private void InventoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      //although name it removedInventoryList, there is only one removed item from combbox
      IList removedInventoryList = e.RemovedItems;
      ComboBox comboBox = (ComboBox)sender;

      if (removedInventoryList.Count != 0)
      {
        string previousComboBoxOption = (string)removedInventoryList[0];
        AddOptionsToOtherComboBox(previousComboBoxOption, comboBox);
      }

      RemoveOptionsfromOtherComboBox(comboBox);

      foreach (Inventory inventory in inventoryList)
      {
        if (inventory.Name.Equals(comboBox.SelectedItem))
        {
          ((TextBlock)((WrapPanel)((FrameworkElement)comboBox.Parent).Parent).Children[4]).Text = inventory.Unit;
          break;
        }
      }

      ValidationCheck();
    }

    private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      ValidationCheck();
    }

    private void AddOptionsToOtherComboBox(string option, ComboBox comboBox)
    {
      if (comboBox == null)
      {
        foreach (DependencyRow otherDependencyRow in dependenciesStackpanel.Children)
        {
          ((ObservableCollection<string>)otherDependencyRow.inventoryComboBox.ItemsSource).Add(option);
        }
      }
      else
      {
        foreach (DependencyRow otherDependencyRow in dependenciesStackpanel.Children)
        {
          if (otherDependencyRow.inventoryComboBox != comboBox)
          {
            ((ObservableCollection<string>)otherDependencyRow.inventoryComboBox.ItemsSource).Add(option);
          }

        }
      }

      inventoryNameList.Add(option);
    }

    private void RemoveOptionsfromOtherComboBox(ComboBox comboBox)
    {
      foreach (DependencyRow otherDependencyRow in dependenciesStackpanel.Children)
      {
        if (otherDependencyRow.inventoryComboBox != comboBox)
        {
          ((ObservableCollection<String>)otherDependencyRow.inventoryComboBox.ItemsSource).Remove((string)comboBox.SelectedItem);
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
      else
      {
        confrimButton.IsEnabled = false;
      }
    }

    private void AddEventHandlersToDependencyRow(DependencyRow dependencyRow)
    {
      dependencyRow.removeDependencyRowButton.Click += RemoveDependencyRowButton_Click;
      dependencyRow.inventoryComboBox.SelectionChanged += InventoryComboBox_SelectionChanged;
      dependencyRow.quantityTextBox.TextChanged += QuantityTextBox_TextChanged;
    }

    private void ConfrimButton_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}
