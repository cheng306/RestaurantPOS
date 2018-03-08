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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantPOS.Models
{
  /// <summary>
  /// Interaction logic for Circle.xaml
  /// </summary>
  public partial class Circle : UserControl
  {
    public Circle()
    {
      InitializeComponent();
    }

    public Circle(Circle c)
    {
      InitializeComponent();
      this.circleUI.Height = c.circleUI.Height;
      this.circleUI.Width = c.circleUI.Height;
      this.circleUI.Fill = c.circleUI.Fill;
    }

    public bool Added { get; set; }

    public bool Movable { get; set; }

    public Point Center { get; set; }

    public Table Table { get; set; }

    public TextBlock NumberTextBlock
    {
      get { return numberTextBlock; }
      set { numberTextBlock= value; }
    }

      

   }
}
