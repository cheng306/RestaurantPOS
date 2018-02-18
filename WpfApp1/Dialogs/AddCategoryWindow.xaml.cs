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

namespace WpfApp1.Dialogs
{
    /// <summary>
    /// Interaction logic for AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        public AddCategoryWindow()
        {
            InitializeComponent();
            InitializeOther();
        }

        private void InitializeOther()
        {
            addButton.IsEnabled = false;
            categoryWarningTextBlock.Visibility = Visibility.Hidden;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            inputTextBox.Focus();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (inputTextBox.Text.Equals(""))
            {
                addButton.IsEnabled = false;
                categoryWarningTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                addButton.IsEnabled = true;
                categoryWarningTextBlock.Visibility = Visibility.Hidden;
            }
            
        }

       

        public string Input
        {
            get { return inputTextBox.Text; }
        }
    }
}
