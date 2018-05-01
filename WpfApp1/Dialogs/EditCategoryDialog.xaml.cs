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
  /// Interaction logic for AddCategoryWindow.xaml
  /// </summary>
  public partial class EditCategoryDialog : Window
  {
    string currentCategory;
    ObservableCollection<string> categoriesList = ((MainWindow)(Application.Current.MainWindow)).editPage.categoriesList;

    public EditCategoryDialog()
    {
      InitializeComponent();
      InitializeOther();
      currentCategory = "";
    }

    public EditCategoryDialog(string currentCategory)
    {
      InitializeComponent();
      InitializeOther();
      this.currentCategory = currentCategory;
    }

    private void InitializeOther()
    {
      addButton.IsEnabled = false;
      categoryWarningTextBlock.Visibility = Visibility.Visible;
    }

    private void Window_ContentRendered(object sender, EventArgs e)
    {
      inputTextBox.Focus();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
    }

    private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (inputTextBox.Text.Equals(""))
      {
        addButton.IsEnabled = false;
        categoryWarningTextBlock.Text = "Category Cannot be Blank";
        categoryWarningTextBlock.Visibility = Visibility.Visible;
      }
      else
      {
        bool repeatedCategory = false;
        foreach (string category in categoriesList)
        {
          if (inputTextBox.Text.Equals(category) && !category.Equals(currentCategory))
          {
            addButton.IsEnabled = false;
            categoryWarningTextBlock.Text = inputTextBox.Text + " already exists";
            categoryWarningTextBlock.Visibility = Visibility.Visible;
            repeatedCategory = true;
            break;
          }
        }
        if (!repeatedCategory)
        {
          addButton.IsEnabled = true;
          categoryWarningTextBlock.Visibility = Visibility.Hidden;
        }
        
      }

    }



    public string Input
    {
      get { return inputTextBox.Text; }
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter && addButton.IsEnabled)
      {
        this.DialogResult = true;
      }
    }
  }
}
