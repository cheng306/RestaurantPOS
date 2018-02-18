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
using WpfApp1.Pages;
using WpfApp1.Dialogs.Templates;

namespace WpfApp1.Dialogs
{
    /// <summary>
    /// Interaction logic for AddInventoryConsumption.xaml
    /// </summary>
    public partial class AddInventoryConsumptionDialog : Window
    {
        public AddInventoryConsumptionDialog()
        {
            InitializeComponent();
        }
        UserControl uc = ((MainWindow)Application.Current.MainWindow).inventoryPage;
        private void AddDependencyButton_Click(object sender, RoutedEventArgs e)
        {
            WrapPanel denpendencyWrapPanel = new WrapPanel();
            denpendencyWrapPanel.HorizontalAlignment = HorizontalAlignment.Stretch;

            denpendencyWrapPanel.Children.Add(new TextBlock {
                Text ="Inventory",
                FontSize =25.0,
                Margin = new Thickness(5.0)
            });

            denpendencyWrapPanel.Children.Add(new ComboBox()
            {
                ItemsSource = ((MainWindow)Application.Current.MainWindow).inventoryPage.inventoryList,
                DisplayMemberPath = "Name",
                FontSize = 25.0,
                Width = 150.0,
                Margin = new Thickness(5.0)
            });

            denpendencyWrapPanel.Children.Add(new TextBlock()
            {
                Text = "Consume Quantity:",
                FontSize = 25.0,
                Margin = new Thickness(5.0)
            });

            denpendencyWrapPanel.Children.Add(new TextBox()
            {
                FontSize = 25.0,
                Width = 150.0,
                Margin = new Thickness(5.0)
            });

            DependencyRow dr = new DependencyRow();

            dependenciesStackpanel.Children.Add(dr);
        }
    }
}
