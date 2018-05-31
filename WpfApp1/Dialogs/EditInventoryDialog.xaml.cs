using RestaurantPOS.Models;
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
using System.Windows.Shapes;

namespace RestaurantPOS.Dialogs
{
  /// <summary>
  /// Interaction logic for AddInventoryWindow.xaml
  /// </summary>
  public partial class EditInventoryDialog : Window
  {
    bool validName;
    bool validQuantity;
    bool validUnit;
    string modifyInventoryName;
    ObservableCollection<Inventory> inventoryList = ((MainWindow)Application.Current.MainWindow).inventoryPage.inventoryList;

    public EditInventoryDialog()
    {
      InitializeComponent();
      nameWarningTextBlock.Visibility = Visibility.Visible;
      quantityWarningTextBlock.Visibility = Visibility.Visible;
      unitWarningTextBlock.Visibility = Visibility.Visible;
      addButton.IsEnabled = false;

      modifyInventoryName = "";
      
    }

    public EditInventoryDialog(Inventory inventory)
    {
      InitializeComponent();

      modifyInventoryName = inventory.Name;

      nameWarningTextBlock.Visibility = Visibility.Hidden;
      quantityWarningTextBlock.Visibility = Visibility.Hidden;
      unitWarningTextBlock.Visibility = Visibility.Hidden;
      Title = "Modify Inventory";
      addButton.Content = "Modify";
      nameTextBox.Text = inventory.Name;
      quantityTextBox.Text = inventory.Quantity.ToString();
      unitTextBox.Text = inventory.Unit;
      addButton.IsEnabled = true;
  
    }

    private void OtherInitialSetup()
    {
      
      addButton.IsEnabled = false;
      //inventoryList = ((MainWindow)Application.Current.MainWindow).inventoryPage.inventoryList;
    }

    internal string InventoryName
    {
      get { return nameTextBox.Text; }
    }

    internal double InventoryQuantity
    {
      get { return Double.Parse(quantityTextBox.Text); }
    }

    internal string InventoryUnit
    {
      get { return unitTextBox.Text; }
    }

    private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (!nameTextBox.Text.Equals(""))
      {
        validName = true;
        foreach (Inventory inventory in inventoryList)
        {
          if (inventory.Name.Equals(nameTextBox.Text) && !modifyInventoryName.Equals(nameTextBox.Text))
          {
            validName = false;
            nameWarningTextBlock.Text = nameTextBox.Text + " already exist";
            nameWarningTextBlock.Visibility = Visibility.Visible;
            break;
          }
        }
        if (validName)
        {
          nameWarningTextBlock.Visibility = Visibility.Hidden;
        }
      }
      else
      {
        validName = false;
        nameWarningTextBlock.Text = "Inventory Name Cannot be Blank";
        nameWarningTextBlock.Visibility = Visibility.Visible;
      }

      UpdateAddButton();
    }

    private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (Double.TryParse(quantityTextBox.Text, out double dump))
      {
        validQuantity = true;
        quantityWarningTextBlock.Visibility = Visibility.Hidden;
      }
      else
      {
        validQuantity = false;
        quantityWarningTextBlock.Visibility = Visibility.Visible;
      }

      UpdateAddButton();
    }

    private void UnitTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (!unitTextBox.Text.Equals(""))
      {
        validUnit = true;
        unitWarningTextBlock.Visibility = Visibility.Hidden;
      }
      else
      {
        validUnit = false;
        unitWarningTextBlock.Visibility = Visibility.Visible;
      }
      UpdateAddButton();
    }

    private void UpdateAddButton()
    {
      if (validName && validQuantity && validUnit)
      {
        addButton.IsEnabled = true;
      }
      else
      {
        addButton.IsEnabled = false;
      }
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
    }


  }
}
