using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RestaurantPOS.Converters;
using RestaurantPOS.Models;
using System.Collections.ObjectModel;
using RestaurantPOS.Pages;
using RestaurantPOS.Dictionaries;
using System.IO;
using System.Xml.Serialization;

namespace RestaurantPOS
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
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

    //All App Object
    internal List<Models.Table> tablesList;
    internal ObservableCollection<Item> itemsList;
    internal ObservableCollection<string> categoriesList;
    internal ObservableCollection<Inventory> inventoryList;
    internal List<bool> tableNumberBooleanList;

    // All dictionary
    internal Dictionary<string, List<Item>> inventoryNameItemsListDict;
    internal Dictionary<string, List<Item>> categoryItemsListDict;
    internal InventoryNameObjectDict inventoryNameObjectDict;
    internal ItemNameObjectDict itemNameObjectDict;
   
    

    public App()
    {
      Console.WriteLine("=================App started");

      //Create the directory for database if not exist
      CreateDbDirectory();

      //locate the path and initialize the serializer
      LoadItemsList();
      LoadCategoriesList();
      LoadInventoryList();
      LoadTableNumberBooleanList();
      LoadTablesList();



      //Build parts that require deserialized objects
      BuildCategoryItemDict();
      BuildInventoryNameItemsListDict();
      inventoryNameObjectDict = new InventoryNameObjectDict();
      itemNameObjectDict = new ItemNameObjectDict();

    }

    



    internal void BuildCategoryItemDict()
    {
      categoryItemsListDict = new Dictionary<string, List<Item>>();
      foreach (String category in categoriesList)
      {
        categoryItemsListDict.Add(category, new List<Item>());
      }
      foreach (Item item in itemsList)
      {
        categoryItemsListDict[item.Category].Add(item);
        Console.WriteLine("Testing: item - " + item.Name + " category: " + item.Category);
      }

    }

    internal void AddCategoryToCategoryItemDict(string category)
    {
      categoryItemsListDict[category] = new List<Item>();
    }

    //make sure oldCategory!= newCategory
    internal void ModifyCategoryInCategoryItemDict(string oldCategory, string newCategory)
    {
      categoryItemsListDict[newCategory] = categoryItemsListDict[oldCategory];
      if (!newCategory.Equals(oldCategory)){
        categoryItemsListDict.Remove(oldCategory);
      }
    }

    internal void RemoveCategoryFromCategoryItemDict(string category)
    {
      categoryItemsListDict.Remove(category);
    }

    internal void AddItemToCategoryItemDict(string category, Item item)
    {
      categoryItemsListDict[category].Add(item);
    }

    internal void ChangeItemCategoryInCategoryItemDict(Item item, string oldCategory, string newCategory)
    {
      categoryItemsListDict[oldCategory].Remove(item);
      categoryItemsListDict[newCategory].Add(item);
    }

    internal void RemoveItemFromCategoryItemDict(Item item)
    {
      categoryItemsListDict[item.Category].Remove(item);
    }

    public Dictionary<string, List<Item>> CategoryItemsDict
    {
      get { return this.categoryItemsListDict; }
    }
    //above are about categoryItemsListDict

    public void BuildInventoryNameItemsListDict()
    {
      inventoryNameItemsListDict = new Dictionary<string, List<Item>>();
      foreach (Inventory inventory in inventoryList)
      {
        inventoryNameItemsListDict[inventory.Name] = new List<Item>();
      }
      foreach (Item item in itemsList)
      {
        foreach (InventoryConsumption inventoryConsumption in item.InventoryConsumptionList)
        {
          inventoryNameItemsListDict[inventoryConsumption.InventoryName].Add(item);
        }
      }
    }

    public void AddInventoryToInventoryNameItemsListDict(Inventory inventory)
    {
      inventoryNameItemsListDict[inventory.Name] = new List<Item>();
    }

    public void EditInventoryNameInInventoryNameItemsListDict(string oldInventoryName, string newInventoryName)
    {
      inventoryNameItemsListDict[newInventoryName] = inventoryNameItemsListDict[oldInventoryName];
      inventoryNameItemsListDict.Remove(oldInventoryName);
    }

    public void RemoveInventoryFromInventoryNameItemsListDict(Inventory inventory)
    {
      inventoryNameItemsListDict.Remove(inventory.Name);
    }

    public void AddItemToInventoryNameItemsListDict(Item item, string inventoryName)
    {
      inventoryNameItemsListDict[inventoryName].Add(item);
    }

    public void RemoveItemFromInventoryNameItemsListDict(Item item)
    {
      foreach (InventoryConsumption inventoryConsumption in item.InventoryConsumptionList)
      {
        inventoryNameItemsListDict[inventoryConsumption.InventoryName].Remove(item);
      }
    }

    public Dictionary<string,List<Item>> InventoryNameItemsListDict
    {
      get { return this.inventoryNameItemsListDict; }
    }

    internal InventoryNameObjectDict InventoryNameObjectDict
    {
      get { return this.inventoryNameObjectDict; }
      set { this.inventoryNameObjectDict = value; }
    }

    internal ItemNameObjectDict ItemNameObjectDict
    {
      get { return this.itemNameObjectDict; }
      set { this.itemNameObjectDict = value; }
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
        tablesList = (List<Models.Table>)tablesListSerializer.Deserialize(streamReader);
        streamReader.Close();
        Console.WriteLine("=============tablesList in tablesPage loaded");
      }
      else
      {
        tablesList = new List<Models.Table>();
        Console.WriteLine("=============tablesList in tablesPage created");
      }
      
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
        itemsList = (ObservableCollection<Item>)listSerializer.Deserialize(streamReader);
        streamReader.Close();
        Console.WriteLine("=================itemsList in editPage loaded");
      }
      else
      {
        itemsList = new ObservableCollection<Item>();
        Console.WriteLine("=============itemList created");
      }
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
        categoriesList = (ObservableCollection<string>)categoriesListSerializer.Deserialize(streamReader);
        streamReader.Close();
        Console.WriteLine("============categoriesList loaded");
      }
      else
      {
        categoriesList = new ObservableCollection<string>();
        Console.WriteLine("===========categoriesList created");
      }
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
        inventoryList = (ObservableCollection<Inventory>)inventoryListSerializer.Deserialize(streamReader);
        streamReader.Close();
        Console.WriteLine("=============inventoryList in inventoryPage loaded");
      }
      else
      {
        inventoryList = new ObservableCollection<Inventory>();
        Console.WriteLine("=============inventoryList in inventoryPage created");
      }
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
        tableNumberBooleanList = (List<bool>)tableNumberBooleanListSerializer.Deserialize(streamReader);
        streamReader.Close();
        Console.WriteLine("=============tableNumberBooleanList in tablesPage loaded");
      }
      else
      {
        tableNumberBooleanList = new List<bool>();
        Console.WriteLine("=============tableNumberBooleanList in tablesPage created");
      }
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
      FileStream fs = File.Create(itemListXmlPath);
      listSerializer.Serialize(fs, itemsList);
      fs.Close();

      fs = File.Create(categoriesListXmlPath);
      categoriesListSerializer.Serialize(fs, categoriesList);
      fs.Close();

      fs = File.Create(inventoryListXmlPath);
      inventoryListSerializer.Serialize(fs, inventoryList);
      fs.Close();

      fs = File.Create(tablesListXmlPath);
      tablesListSerializer.Serialize(fs, tablesList);
      fs.Close();

      fs = File.Create(tableNumberBooleanListXmlPath);
      tableNumberBooleanListSerializer.Serialize(fs, tableNumberBooleanList);
      fs.Close();
    }



  }
}
