using RestaurantPOS.Dictionaries;
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
    static MainWindow mainWindow = (MainWindow)currentApp.MainWindow;
    static ObservableCollection<string> categoriesList = currentApp.categoriesList;
    

    public CategoriesWrapPanel()
    {

      foreach (string categoryStr in categoriesList)
      {
        Button categoryButton = new Button
        {
          Content = categoryStr,
          Background = currentApp.antiqueWhiteBrush,
          Margin = new Thickness(10),
          Width = 150,
          Height = 100
        };
        categoryButton.Click += CategoryButton_Click;
        this.Children.Add(categoryButton);
      }
    }

    private void CategoryButton_Click(object sender, RoutedEventArgs e)
    {
      Console.WriteLine("=============CategoryButton Click");
      Button categoryButton = (Button)sender;
      WrapPanel itemsWrapPanel = mainWindow.itemsSelectionPage.CategoryItemsWrapPanelDict[categoryButton.Content.ToString()];

      mainWindow.itemsSelectionPage.wrapPanelScrollViewer.Content = itemsWrapPanel;
      mainWindow.itemsSelectionPage.backToCategoriesButton.Visibility = Visibility.Visible;
    }

    internal void AddCategoryToCategoriesWrapPanel(string categoryName)
    {
      Button categoryButton = new Button
      {
        Content = categoryName,
        Background = currentApp.antiqueWhiteBrush,
        Margin = new Thickness(10),
        Width = 150,
        Height = 100
      };

      categoryButton.Click += CategoryButton_Click;
      this.Children.Add(categoryButton);
    }

    internal void ModifyCategoryInCategoryWrapPanel(string oldCategory, string newCategory)
    {
      for (int i = 0; i < this.Children.Count; i++)
      {
        if (((Button)this.Children[i]).Content.ToString().Equals(oldCategory))
        {
          ((Button)this.Children[i]).Content = newCategory;
          break;
        }
      }
    
    }

    internal void RemoveCategoryFromCategoriesWrapPanel(string categoryName)
    {
      for (int i = 0; i < this.Children.Count; i++)
      {
        if (((Button)this.Children[i]).Content.ToString().Equals(categoryName))
        {
          this.Children.Remove(this.Children[i]);
          break;
        }
      }
    }

  }
}
