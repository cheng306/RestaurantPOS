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

namespace WpfApp1.Windows
{
    /// <summary>
    /// Interaction logic for AddInventoryConsumption.xaml
    /// </summary>
    public partial class AddInventoryConsumptionWindow : Window
    {
        public AddInventoryConsumptionWindow()
        {
            InitializeComponent();
        }

        private void AddDependencyButton_Click(object sender, RoutedEventArgs e)
        {
            WrapPanel denpendencyWrapPanel = new WrapPanel();
            denpendencyWrapPanel.Children.Add(new TextBlock { Text="Inventory"});
            denpendencyWrapPanel.Children.Add(new ComboBox());
            denpendencyWrapPanel.Children.Add(new TextBlock { Text = "Consume Quantity" });
            denpendencyWrapPanel.Children.Add(new TextBox());

            dependenciesStackpanel.Children.Add(denpendencyWrapPanel);
        }
    }
}
