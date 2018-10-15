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
  /// Interaction logic for MessageBoxDialog.xaml
  /// </summary>
  public partial class MessageBoxDialog : Window
  {
    public MessageBoxDialog()
    {
      InitializeComponent();
    }

    public MessageBoxDialog(string message)
    {
      InitializeComponent();
      messageTextBlock.Text = message;
    }

    private void ConfrimButton_Click(object sender, RoutedEventArgs e)
    {
      Console.WriteLine("======ConfrimButton_Click======");
      e.Handled = true;
      this.Close();
    }

    private void ConfrimButton_TouchDown(object sender, TouchEventArgs e)
    {
      e.Handled = true;
      this.Close();
    }
  }
}
