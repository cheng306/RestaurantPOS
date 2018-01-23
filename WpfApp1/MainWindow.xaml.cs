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
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
  
        //path of the itemlist.xml
        String itemListXmlPath;

        //Serializer used to serialize and deserialize ObservableCollection<Item>
        XmlSerializer listSerializer;

        public MainWindow()
        {
            InitializeComponent();

            Console.WriteLine("================Window start==================");
            
            //open or create the directory
            DirectoryInfo dbDirectory = new DirectoryInfo("db");
            if (!dbDirectory.Exists)
            {
                dbDirectory.Create();
            }

            //locate the path and initialize the serializer
            itemListXmlPath = @"db\itemlist.xml";
            listSerializer = new XmlSerializer(typeof(ObservableCollection<Item>));


            if (File.Exists(itemListXmlPath))
            {
                
                StreamReader streamReader = new StreamReader(itemListXmlPath);
                editPage.itemList = (ObservableCollection<Item>)listSerializer.Deserialize(streamReader);
                streamReader.Close();

                Console.WriteLine("itemList loaded");
            }
            else
            {
                
                editPage.itemList = new ObservableCollection<Item>();

                Console.WriteLine("itemList created");
            }



            editPage.itemList.Add(new Item { Name = "asd", Category="asds", Price = 12.6 });
            editPage.itemList.Add(new Item { Name = "asd", Category = "asfdgds", Price = 12.6 });
            editPage.itemList.Add(new Item { Name = "asd", Category = "asdfgds", Price = 12.6 });

            editPage.itemListView.ItemsSource = editPage.itemList;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {     
            FileStream fs = File.Create(itemListXmlPath);
            ObservableCollection<Item> list = editPage.itemList;

            listSerializer.Serialize(fs, list);
            fs.Close();
        }
    }
}
