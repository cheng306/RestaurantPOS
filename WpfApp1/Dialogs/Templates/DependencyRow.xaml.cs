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

namespace WpfApp1.Dialogs.Templates
{
    /// <summary>
    /// Interaction logic for DependencyRow.xaml
    /// </summary>
    public partial class DependencyRow : UserControl
    {
        

        public DependencyRow()
        {
            InitializeComponent();
            inventoryComboBox.ItemsSource = ((MainWindow)Application.Current.MainWindow).inventoryPage.inventoryList;
        }



        
    }
}
