using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml.Serialization;
using RestaurantPOS.Models;
using RestaurantPOS.Pages.Templates;

namespace RestaurantPOS
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  /// 


  public partial class MainWindow : Window
  {
    //path of xml file that store itemsList in Edit Page
    //Serializer used to serialize and deserialize ObservableCollection<Item>
    string itemListXmlPath;
    XmlSerializer listSerializer;

    //path of xml file that store categoriesList in Edit Page
    //Serializer used to serialize and deserialize ObservableCollection<String>
    string categoriesListXmlPath;
    XmlSerializer categoriesListSerializer;

    //path of xml file that store inventoryList in Inventory Page
    //Serializer used to serialize and deserialize ObservableCollection<Inventory>
    string inventoryListXmlPath;
    XmlSerializer inventoryListSerializer;

    //path of xml file that store tablesList in tables
    //Serializer used to serialize and deserialize List<Circle>
    string tablesListXmlPath;
    XmlSerializer tablesListSerializer;

    //path of xml file that store tableNumberBooleanList in tablesPage
    //Serializer used to serialize and deserialize List<bool>
    string tableNumberBooleanListXmlPath;
    XmlSerializer tableNumberBooleanListSerializer;



    public MainWindow()
    {
      InitializeComponent();

      Console.WriteLine("================Window created==================");

      //Create the directory for database if not exist
      CreateDbDirectory();

      //locate the path and initialize the serializer
      LoadItemsList();
      LoadCategoriesList();
      LoadInventoryList();
      LoadTableNumberBooleanList();
      LoadTablesList();

      //Build parts that require deserialized objects
      itemsSelectionPage.BuildCategoryItemsWrapPanelDictionary();
      itemsSelectionPage.BuildCategoriesWrapPanel();

      inventoryPage.BuildNameInventoryDict();
      ((App)Application.Current).BuildCategoryItemDict();
      ((App)Application.Current).BuildInventoryItemsDict();
      ((App)Application.Current).BuildItemNameCategoryObjectDict();

    }

    private void CreateDbDirectory()
    {
      DirectoryInfo dbDirectory = new DirectoryInfo("db");
      if (!dbDirectory.Exists)
      {
        dbDirectory.Create();
      }
    }

    private void LoadTablesList()
    {
      //locate the path
      tablesListXmlPath = @"db\tableslist.xml";

      //initilize serizlizer
      tablesListSerializer = new XmlSerializer(typeof(List<Models.Table>));

      if (File.Exists(tablesListXmlPath))
      {
        StreamReader streamReader = new StreamReader(tablesListXmlPath);
        tablesPage.tablesList = (List<Models.Table>)tablesListSerializer.Deserialize(streamReader);
        streamReader.Close();
        Console.WriteLine("=============tablesList in tablesPage loaded");
      }
      else
      {
        tablesPage.tablesList = new List<Models.Table>();
        Console.WriteLine("=============tablesList in tablesPage created");
      }
      
      tablesPage.LoadTables();
    }

    private void LoadItemsList()
    {
      //locate the path
      itemListXmlPath = @"db\itemlist.xml";

      //initilize serizlizer
      listSerializer = new XmlSerializer(typeof(ObservableCollection<Item>));

      if (File.Exists(itemListXmlPath))
      {
        StreamReader streamReader = new StreamReader(itemListXmlPath);
        editPage.itemsList = (ObservableCollection<Item>)listSerializer.Deserialize(streamReader);
        streamReader.Close();
        Console.WriteLine("itemList loaded");
      }
      else
      {
        editPage.itemsList = new ObservableCollection<Item>();
        Console.WriteLine("=============itemList created");
      }

      editPage.itemsListView.ItemsSource = editPage.itemsList;
      inventoryPage.itemsListView.ItemsSource = editPage.itemsList;
    }

    private void LoadCategoriesList()
    {
      //locate the path
      categoriesListXmlPath = @"db\categoreslist.xml";

      //initilize serizlizer
      categoriesListSerializer = new XmlSerializer(typeof(ObservableCollection<string>));

      if (File.Exists(categoriesListXmlPath))
      {
        StreamReader streamReader = new StreamReader(categoriesListXmlPath);
        editPage.categoriesList = (ObservableCollection<string>)categoriesListSerializer.Deserialize(streamReader);
        streamReader.Close();
        Console.WriteLine("============categoriesList loaded");
      }
      else
      {
        editPage.categoriesList = new ObservableCollection<string>();
        Console.WriteLine("===========categoriesList created");
      }

      editPage.categoriesListBox.ItemsSource = editPage.categoriesList;
    }

    private void LoadInventoryList()
    {
      //locate the path
      inventoryListXmlPath = @"db\inventorylist.xml";

      //initilize serizlizer
      inventoryListSerializer = new XmlSerializer(typeof(ObservableCollection<Inventory>));

      if (File.Exists(inventoryListXmlPath))
      {
        StreamReader streamReader = new StreamReader(inventoryListXmlPath);
        inventoryPage.inventoryList = (ObservableCollection<Inventory>)inventoryListSerializer.Deserialize(streamReader);
        streamReader.Close();
        Console.WriteLine("=============inventoryList in inventoryPage loaded");
      }
      else
      {
        inventoryPage.inventoryList = new ObservableCollection<Inventory>();
        Console.WriteLine("=============inventoryList in inventoryPage created");
      }

      inventoryPage.inventoryListView.ItemsSource = inventoryPage.inventoryList;
    }

    private void LoadTableNumberBooleanList()
    {
      //locate the path
      tableNumberBooleanListXmlPath = @"db\tablenumberbooleanlist.xml";

      //initilize serizlizer
      tableNumberBooleanListSerializer = new XmlSerializer(typeof(List<bool>));

      if (File.Exists(tableNumberBooleanListXmlPath))
      {
        StreamReader streamReader = new StreamReader(tableNumberBooleanListXmlPath);
        tablesPage.tableNumberBooleanList = (List<bool>)tableNumberBooleanListSerializer.Deserialize(streamReader);
        streamReader.Close();
        Console.WriteLine("=============tableNumberBooleanList in tablesPage loaded");
      }
      else
      {
        tablesPage.tableNumberBooleanList = new List<bool>();
        Console.WriteLine("=============tableNumberBooleanList in tablesPage created");
      }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      FileStream fs = File.Create(itemListXmlPath);
      ObservableCollection<Item> list = editPage.itemsList;
      listSerializer.Serialize(fs, list);
      fs.Close();

      fs = File.Create(categoriesListXmlPath);
      ObservableCollection<string> categoriesList = editPage.categoriesList;
      categoriesListSerializer.Serialize(fs, categoriesList);
      fs.Close();

      fs = File.Create(inventoryListXmlPath);
      ObservableCollection<Inventory> inventoryList = inventoryPage.inventoryList;
      inventoryListSerializer.Serialize(fs, inventoryList);
      fs.Close();

      fs = File.Create(tablesListXmlPath);
      List<Models.Table> tableList = tablesPage.tablesList;
      tablesListSerializer.Serialize(fs, tableList);
      fs.Close();

      fs = File.Create(tableNumberBooleanListXmlPath);
      List<bool> tableNumberBooleanList = tablesPage.tableNumberBooleanList;
      tableNumberBooleanListSerializer.Serialize(fs, tableNumberBooleanList);
      fs.Close();
    }


  }
}
