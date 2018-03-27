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
using RestaurantPOS.Models;

namespace RestaurantPOS.Pages
{
  /// <summary>
  /// Interaction logic for TestPage.xaml
  /// </summary>
  public partial class SelectionPage : UserControl
  {
    public SelectionPage()
    {
      InitializeComponent();
      Console.WriteLine("InitializeComponent");
      //CategoryWrapPanel();

    }

    public SelectionPage(Circle tableUI)
    {
      InitializeComponent();
      TableNumberTextBlock.Text = "Table " + tableUI.Table.TableNumber;
      FillCategoryWrapPanel();

    }

    private void FillCategoryWrapPanel()
    {
      MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
      ObservableCollection<string> categoriesList = mainWindow.editPage.categoriesList;
      foreach(string categoryStr in categoriesList)
      {
        Button categoryButton = new Button { Content = categoryStr };
        categoryButton.Margin = new Thickness(10);
        categoryButton.Width = 150;
        categoryButton.Height = 100;
        selectionWrapPanel.Children.Add(categoryButton);
      }
    }
  



 
  }
}
