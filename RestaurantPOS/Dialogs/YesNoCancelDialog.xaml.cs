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

namespace RestaurantPOS.Dialogs
{
  /// <summary>
  /// Interaction logic for YesNoCanvelDialog.xaml
  /// </summary>
  public partial class YesNoCancelDialog : Window
  {
    public YesNoCancelDialog(string message)
    {
      InitializeComponent();
      messageTextBlock.Text = message;
    }

    //combine both click and touchdown
    //private void YesButton_MouseOrTouchDown(object sender, EventArgs e)
    //{
    //  DialogResult = true;
    //}

    private void YesButton_Click(object sender, RoutedEventArgs e)
    {
      Console.WriteLine("======YesButton_Click======");
      DialogResult = true;
      e.Handled = true;
    }

    private void YesButton_TouchUp(object sender, TouchEventArgs e)
    {
      Console.WriteLine("======YesButton_TouchUp======");
      DialogResult = true;
      e.Handled = true;
    }

    private void NoOrCancelButton_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      e.Handled = true;
    }

    private void NoOrCancelButton_TouchUp(object sender, TouchEventArgs e)
    {
      DialogResult = false;
      e.Handled = true;
    }




   
    
  }
}
