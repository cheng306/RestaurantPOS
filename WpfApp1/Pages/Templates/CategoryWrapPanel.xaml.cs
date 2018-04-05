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

namespace RestaurantPOS.Pages.Templates
{
  /// <summary>
  /// Interaction logic for CategoryWrapPanel.xaml
  /// </summary>
  public partial class CategoryWrapPanel : UserControl
  {
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    public CategoryWrapPanel()
    {
      InitializeComponent();
      
    }

    private void CategoryButton_Click(object sender, RoutedEventArgs e)
    {
      throw new NotImplementedException();
    }
  }
}
