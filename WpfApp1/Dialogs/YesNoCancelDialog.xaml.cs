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

    private void NoOrCancelButton_MouseOrTouchDown(object sender, EventArgs e)
    {
      DialogResult = false;
    }

    private void YesButton_MouseOrTouchDown(object sender, EventArgs e)
    {
      DialogResult = true;
    }
  }
}
