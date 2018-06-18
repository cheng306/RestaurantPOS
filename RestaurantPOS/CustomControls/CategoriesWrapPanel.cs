using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RestaurantPOS.Models
{
  internal class CategoriesWrapPanel:WrapPanel
  {
    static App currentApp = (App)Application.Current;
    static ObservableCollection<string> categoriesList = currentApp.categoriesList;
    SolidColorBrush antiqueWhiteBrush = new SolidColorBrush(Colors.AntiqueWhite);

    public CategoriesWrapPanel()
    {
      foreach (string categoryStr in categoriesList)
      {
        Button categoryButton = new Button
        {
          Content = categoryStr,
          Background = antiqueWhiteBrush,
          Margin = new Thickness(10),
          Width = 150,
          Height = 100
        };
        //categoryButton.Click += CategoryButton_Click;
        this.Children.Add(categoryButton);
      }
    }

  }
}
