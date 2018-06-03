using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml.Serialization;
using RestaurantPOS.Models;
using RestaurantPOS.Dictionaries;
using RestaurantPOS.Pages.Templates;

namespace RestaurantPOS
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  /// 
 

  public partial class MainWindow : Window
  {
    

    App currentApp = (App)Application.Current;

    public MainWindow()
    {
      InitializeComponent();

      //build divtionary
      itemsSelectionPage.BuildCategoryItemsWrapPanelDictionary();
      itemsSelectionPage.BuildCategoriesWrapPanel();

      Console.WriteLine("================Window created==================");
    }

    

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      
    }


  }
}
